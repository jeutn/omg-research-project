using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ProductionManager : MonoBehaviour
{
    public static ProductionManager Instance { get; private set; }

    private int goodsProduced;

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
        BaseProduction(player, prodData);

        //only if base production has occurred, then trigger production chain - HAVE UI where player can choose to proceed with production chain or not 
        if (prodData.productionChain != ResourceType.none && CanChain(player, prodData))
        {
            //popup to ask if player wants to proceed with production chain 
        }

        ProductionChain(player, prodData); //empty method 
        ResolveConstruction(player); 
        
    }

    //STEP 1. BASE PRODUCTION - method to confirm production - discard selected hand cards + resolves output and economies
    private void BaseProduction(Player player, ProdCardData building)
    {
        //1. consume resources based on worker mode 
        //2. add goods to player goods inventory
        //3. receive coins 

        //if all conditions are met:
        foreach (Card card in player.selectedResources)
        {
            player.playerHand.Remove(card);
            Deck.Instance.DiscardCard(player, card);
        }
        
        player.selectedResources.Clear();
        player.goodsInventory[building.prodOutput] += goodsProduced;
        player.coins += ResourceCoinValues.Value[building.prodOutput] * goodsProduced;
        Debug.Log("successfully produced");
        
    }

    //STEP 2. PRODUCTION CHAIN 
    //popup to ask player if they would like to proceed with production chain 
    private void ProductionChain(Player player, ProdCardData building)
    {
        //1. consume additional resources if conditions met - for every additional/set of additional resources, produce extra goods
        ResourceType chainResource = building.productionChain; 
        int chainCount = AvailableResourceCount(player, chainResource); //count from hand and market
        
        List<Card> toDiscard = player.selectedResources
        .Where(card => card.cardData.rawResource == chainResource)
        .ToList(); //discard all matching selected cards

        foreach (Card card in toDiscard)
        {
            player.playerHand.Remove(card);
            Deck.Instance.DiscardCard(player, card);
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
        CardData data = player.queuedBuilding.cardData;
        
        //2. deduct coins/goods 
        //how to pay with goods - player has visual stack of resources? then they must be scriptable objects then? - click buildings in hand and check the output 

        // check player can afford it
        if (player.coins < data.costToBuild) return;
        // need to check which goods player wants to pay with 

        //deduct costs
        player.coins -= data.costToBuild;
        //deduct goods from inventory 

        //3. move building to building site 
        player.buildingSite.Add(player.queuedBuilding);
        player.queuedBuilding = null;
    }

    //HELPER METHODS  
    private int DetermineGoodsAmt(Player player)
    {
        //ternary operator, if efficient worker, goodsproduced: 2, if sloppy: 1
        return goodsProduced = player.workerMode == WorkerMode.Efficient ? 2 : 1;
    }
    private bool CanProduceWithSelected(Player player, Card building)
    {
        // check player hand + market display for required resources
        // make sure to use ProdCardData prodData from building;

        if (building.cardData is ProdCardData prodData) //casting to prodData, will have to cast every time 
        {
            foreach (ProdCardData.ResourceAmts input in prodData.productionInput)
            {
                ResourceType type = input.type;
                int required = input.amount;
                int available = AvailableResourceCount(player, type);
                
                if (available < required)
                {
                    return false;  
                }
            }
            return true;
        }
        return false;
    }

    //method to count amount of resources for each type to determine if there is enough 
    private int AvailableResourceCount(Player player, ResourceType resource)
    {
        int fromSelected = player.selectedResources.Count(card => card.cardData.rawResource == resource);
        int fromMarket = MarketManager.Instance.marketDisplay.Count(card => card.cardData.rawResource == resource);
        int total = fromSelected + fromMarket;
        return total;
    }

    //method to pass production turn - if player chooses to forfeit their turn 
    public void PassProduction(Player player)
    {
        player.selectedResources.Clear();
        PhaseManager.Instance.PlayerTurnEnd();
    } 

    //method to minus 1 if worker mode is sloppy 

    //method - check if production chain is eligible, ie player has resources or market display has resources  
    private bool CanChain(Player player, ProdCardData prodData)
    {
        ResourceType chainResource = prodData.productionChain;
        return AvailableResourceCount(player, chainResource) > 0;
    } 

    //method - resolve construction

    //at the end of production or round - RESET WORKER MODE TO DEFAULT








}
