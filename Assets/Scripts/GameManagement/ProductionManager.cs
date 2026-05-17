using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ProductionManager : MonoBehaviour
{
    public static ProductionManager Instance { get; private set; }

    private int goodsProduced;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResolveProduction(Player player)
    {
        //if production building doesnt exist, can't cast to proddata, then check if there isnt enough resources 
        if (player.queuedBuilding == null || player.queuedBuilding.cardData is not ProdCardData prodData || !CanProduceWithSelected(player, player.queuedBuilding))
        {
            Debug.Log("Cannot produce");
            //need to move on to next player
            return;
        }

        //first resolve base production
        goodsProduced = DetermineGoodsAmt(player);
        BaseProduction(player);

        //only if base production has occurred, then trigger production chain - HAVE UI where player can choose to proceed with production chain or not 
        if (prodData.productionChain != ResourceType.none && CanChain(player, prodData))
        {
            //popup to ask if player wants to proceed with production chain 
        }

        ProductionChain(player);
        ResolveConstruction(player); 
        
    }

    //STEP 1. BASE PRODUCTION - method to confirm production - discard selected hand cards + resolves output and economies
    private void BaseProduction(Player player)
    {
        //1. consume resources based on worker mode 
        //2. add goods to player goods inventory
        //3. receive coins 

        if (player.productionBuilding.cardData is not ProdCardData building) return;

        //if all conditions are met:
        foreach (Card card in player.selectedResources)
        {
            if (card.cardLocation == CardLocation.Hand)
            {
                card.cardLocation = CardLocation.Discard; 
                player.playerHand.Remove(card);
                Deck.Instance.DiscardCard(card); 
            } // market cards are only deselected 
        }
        player.selectedResources.Clear();
        player.activatedOffices.Clear();

        //resolve economies 
        player.goodsInventory[building.prodOutput] += goodsProduced;
        player.coins += ResourceCoinValues.Value[building.prodOutput] * goodsProduced;
        Debug.Log("successfully produced");
        
    }

    //STEP 2. PRODUCTION CHAIN 
    //popup to ask player if they would like to proceed with production chain 
    private void ProductionChain(Player player)
    {
        if (player.productionBuilding.cardData is not ProdCardData building) return;

        //1. consume additional resources if conditions met - for every additional/set of additional resources, produce extra goods
        ResourceType chainResource = building.productionChain; 
        int chainCount = AvailableResourceCount(player, chainResource, chainOnly: true); //count from hand and market
        
        List<Card> toDiscard = player.selectedResources
        .Where(card => card.cardData.rawResource == chainResource)
        .ToList(); //discard all matching selected cards

        foreach (Card card in toDiscard)
        {
            player.playerHand.Remove(card);
            card.cardLocation = CardLocation.Discard;
            Deck.Instance.DiscardCard(card);
        }
        player.selectedResources.Clear();

        //2. add goods to player goods inventory 
        player.goodsInventory[building.prodOutput] += chainCount;
    
        //3. receive coins 
        player.coins += ResourceCoinValues.Value[building.prodOutput] * chainCount;
        
        //move to next turn - DO FOR ALL methods - ONLY CURRENTLY IN THIS ONE 
        PhaseManager.Instance.PlayerTurnEnd();
    }

    //STEP 3. RESOLVE CONSTRUCTION
    private void ResolveConstruction(Player player)
    {
        //1. if player has building queued
        if (player.queuedBuilding == null) return;

        //1.1 if player can afford it with their current goods 
        //popup to say insufficient funds if not enough -> then continue choosing more goods -> popup to say finalise construction or pass turn 
        if (!CanAffordConstruction(player))
        {
            Debug.Log("cannot afford construction - forfeit turn?");
            //BUTTON TO PASS TURN - in passing turn - return card to discard pile 
        }
        
        //2. deduct coins/goods from inventory
        foreach (var good in player.selectedGoods)
        {
            if (good.Key.cardData is not ProdCardData prodData) continue;
            player.goodsInventory[prodData.prodOutput] -= good.Value;
            player.coins -= good.Value * ResourceCoinValues.Value[prodData.prodOutput];
        }

        player.selectedGoods.Clear();

        //2.1. update VPs and update location
        player.victoryPoints += player.queuedBuilding.cardData.victoryPoints;
        player.queuedBuilding.cardLocation = CardLocation.BuildingSite;

        //3. move building to building site 
        player.buildingSite.Add(player.queuedBuilding);
        Debug.Log("successful building constructed");
    }

    //HELPER METHODS - BASE PRODUCTION  
    private int DetermineGoodsAmt(Player player)
    {
        //ternary operator, if efficient worker, goodsproduced: 2, if sloppy: 1 - constants in WorkerManager
        return goodsProduced = player.workerMode == WorkerMode.Efficient ? WorkerManager.Instance.efficientGoods : WorkerManager.Instance.sloppyGoods;
    }
    private bool CanProduceWithSelected(Player player, Card building)
    {
        // check player hand + market display for required resources
        // make sure to use ProdCardData prodData from building;

        if (building.cardData is not ProdCardData prodData) return false;

        int totalDeficit = 0;
        //if sloppy worker, -1 resource means the required-available = 1
        //if efficient worker, deficit = 0

        foreach (ProdCardData.ResourceAmts input in prodData.productionInput)
        {
            int available = AvailableResourceCount(player, input.type);
            int deficit = Mathf.Max(0, input.amount - available);
            totalDeficit += deficit;
        }
        if (player.workerMode == WorkerMode.Efficient) return totalDeficit == 0;
        if (player.workerMode == WorkerMode.Sloppy) return totalDeficit <= 1;
        return false;
    }

    //method to count amount of resources for each type to determine if there is enough (from selected hand and market cards that player chooses), also include MARKET OFFICES from the building site that act as extra resources 
    private int AvailableResourceCount(Player player, ResourceType resource, bool chainOnly = false)
    {

        if (chainOnly) //PRODUCTION CHAIN CANNOT USE OFFICES - make unable to select with UI
        {
            return player.selectedResources.Count(card => card.cardData.rawResource == resource && card.cardLocation == CardLocation.Hand);
        }

        int fromSelected = player.selectedResources.Count(card => card.cardData.rawResource == resource && (card.cardLocation == CardLocation.Market || card.cardLocation == CardLocation.Hand)); //includes selected from market display and hand 

        int fromOffices = player.activatedOffices.Count(card => card.cardData is MarketCardData market && 
                                                                market.cardEffectType == CardEffectType.AddMarketResource && 
                                                                market.bonusResource == resource);
        return fromSelected + fromOffices;
    }

    //method to pass production turn - if player chooses to forfeit their turn 
    public void PassProduction(Player player)
    {
        player.selectedResources.Clear();
        PhaseManager.Instance.PlayerTurnEnd();
    } 

    //method - check if production chain is eligible, ie player has resources or market display has resources - resources player can choose from market display -> NOT AUTOMATICALLY COUNTING MAX  
    private bool CanChain(Player player, ProdCardData prodData)
    {
        ResourceType chainResource = prodData.productionChain;
        return AvailableResourceCount(player, chainResource) > 0;
    } 

    //method to select goods via buildings 
    //method to deselect goods via buildings 

    //method to check if player can afford construction with selected goods - need to wire with button that allows players to add more than one good to the selectedGoods (select building, + and - on top)
    private bool CanAffordConstruction(Player player)
    {
        if (player.queuedBuilding == null) return false;

        int totalCost = 0;
        foreach (var good in player.selectedGoods)
        {
            if (good.Key.cardData is not ProdCardData prodData) continue;
            totalCost += ResourceCoinValues.Value[prodData.prodOutput] * good.Value;
        }

        return totalCost >= player.queuedBuilding.cardData.costToBuild;

    }

}
