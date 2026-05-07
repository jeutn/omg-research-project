using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WorkerMode workerMode;
    public int playerID;
    public List<Card> playerHand;
    public List<Card> buildingSite;
    public Card productionBuilding;
    public int coins;
    //store # amount of each resource type player has 
    public Dictionary<ResourceType, int> goodsInventory = new Dictionary<ResourceType, int>(); //either create this with every resource at the start, or add to it every time they get a new resource 
    public List<Card> selectedResources = new(); //temporary for each round, cards are tracked if players want to use in production - will need select/deselect UI methods 


    private void Awake()
    {
        //populate player goods inventory 
        foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
        {
            goodsInventory[0] = 0;
        }
    }
}
