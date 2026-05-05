using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public List<Card> playerHand;
    public List<Card> buildingSite;
    public Card productionBuilding;
    public int coins;
    //store # amount of each resource type player has 
    public Dictionary<ResourceType, int> goodsInventory = new Dictionary<ResourceType, int>(); //either create this with every resource at the start, or add to it every time they get a new resource 

    //workermode, list for assistant (building, )

    private void Awake()
    {
        foreach (ResourceType resource in System.Enum.GetValues(typeof(ResourceType)))
        {
            goodsInventory[0] = 0;
        }
    }
}
