using UnityEngine;

public abstract class CardData : ScriptableObject
{
    //common across all cards
    public int cardID;
    public CardType cardType;
    public ResourceType rawResource;
    public string cardName;
    public int costToBuild;
    public int victoryPoints;
    public bool halfSun;

}

