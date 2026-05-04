using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public int cardID;
    public CardType cardType;
    public ResourceType rawResource;
    public string cardName;
    public int costToBuild;
    public int victoryPoints;
    public bool halfSun;

}
