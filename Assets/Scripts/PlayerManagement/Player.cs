using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public List<Card> playerHand;
    public List<Card> buildingSite;
    public Card productionBuilding;
    public Card queuedBuilding;
    public WorkerMode workerMode;
    public int coins;
    public int victoryPoints;
    //store # amount of each resource type player has 
    public Dictionary<ResourceType, int> goodsInventory = new Dictionary<ResourceType, int>(); //either create this with every resource at the start, or add to it every time they get a new resource 
    public List<Card> selectedResources = new(); //temporary for each round, cards are tracked if players want to use in production - will need select/deselect UI methods - includes both market display cards and cards in hand 
    public List<Card> activatedOffices = new(); //temporary for each round, if market offices are in play and players want to use their bonus resources, need to select/deselect (does not include the draw extra card market office) //if card == market office, add to this list 
    public Dictionary<Card, int> selectedGoods = new(); //temporary for each round, when players want to pay with goods, tap the building/s 1+ times and adds it to this dictionary


    private void Awake()
    {
        //populate player goods inventory 
        foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
        {
            goodsInventory[resource] = 0;
        }
    }
}
