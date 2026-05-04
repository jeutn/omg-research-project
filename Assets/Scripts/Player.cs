using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;
    public List<Card> hand;
    public List<Card> buildingSite;
    public Card productionBuilding;
    public int coins;
    //store # amount of each resource type player has 
    public Dictionary<ResourceType, int> goodsInventory = new Dictionary<ResourceType, int>();

    //workermode, list for assistant (building, )
}
