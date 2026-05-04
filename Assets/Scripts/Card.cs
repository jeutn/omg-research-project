using UnityEngine;

[RequireComponent(typeof(CardMovement))] //handles everything perceived with card movement
[RequireComponent(typeof(CardUI))] //will automatically attach CardUI script to every card object

public class Card : MonoBehaviour
{
    
    public CardData cardData;
    public bool usedAsResource;
    public bool isBuilt;

    [field: SerializeField] public CardData scriptableData {get; private set;}

    public void SetUp(CardData data)
    {
        scriptableData = data;
        GetComponent<CardUI>().Setup(scriptableData);
    }
    

}
