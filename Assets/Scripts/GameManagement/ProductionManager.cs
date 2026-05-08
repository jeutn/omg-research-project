using UnityEngine;
using System.Linq;

public class ProductionManager : MonoBehaviour
{
    public static ProductionManager Instance { get; private set; }

    private int goodsProduced;

    public void ResolveProduction(Player player, Card building)
    {
        DetermineGoodsAmt(player);
        BaseProduction(player, building);
        ProductionChain(player,building);
        ResolveConstruction(player, player.productionBuilding); //make sure productionBuilding is not the building in the args, is this okay - or make public and call in game manager/phase manager 
        
    }

    //STEP 1. BASE PRODUCTION
    private void BaseProduction(Player player, Card building)
    {
        //1. consume resources based on worker mode 
        //2. add goods to player goods inventory
        //3. receive coins 
        
    }

    //STEP 2. PRODUCTION CHAIN 
    private void ProductionChain(Player player, Card building)
    {
        //1. consume additional resources if conditions met
        //2. add goods to player goods inventory 
        //3. receive coins 
    }

    //STEP 3. RESOLVE CONSTRUCTION
    private void ResolveConstruction(Player player, Card building)
    {
        //1. if player has building queued
        //2. deduct coins/goods 
        //3. move building to building site 
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

    //method to confirm production - discard selected hand cards + resolves output and economies
    public void ConfirmProduction(Player player)
    {
        //if production building doesnt exist, can't cast to proddata, then check if there isnt enough resources 
        if (player.productionBuilding == null || player.productionBuilding.cardData is not ProdCardData prodData || !CanProduceWithSelected(player, player.productionBuilding))
        {
            Debug.Log("Cannot produce");
            //need to move on to next player
            return;
        }

        //if all conditions are met:
        foreach (Card card in player.selectedResources)
        {
            player.playerHand.Remove(card);
            Deck.Instance.DiscardCard(player, card);
        }
        player.selectedResources.Clear();
        int amountProduced = DetermineGoodsAmt(player);
        player.goodsInventory[prodData.prodOutput] += amountProduced;
        player.coins += ResourceCoinValues.Value[prodData.prodOutput] * amountProduced;
    }

    //method to pass production turn - if player chooses to forfeit their turn 
    public void PassProduction(Player player)
    {
        player.selectedResources.Clear();
        PhaseManager.Instance.PlayerTurnEnd();
    } 

    //method to minus 1 if worker mode is sloppy 

    //at the end of production or round - RESET WORKER MODE TO DEFAULT








}
