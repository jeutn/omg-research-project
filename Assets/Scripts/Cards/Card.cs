using UnityEngine;

[RequireComponent(typeof(CardMovement))] //handles everything perceived with card movement
[RequireComponent(typeof(CardUI))] //will automatically attach CardUI script to every card object

public class Card : MonoBehaviour
{
    public Player owner;
    
    public CardData cardData;
    public CardLocation cardLocation;

    public void SetUp(CardData data)
    {
        cardData = data;
        GetComponent<CardUI>().Setup(this);
    }
    

}
