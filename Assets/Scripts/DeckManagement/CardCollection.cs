using System.Collections.Generic;
using UnityEngine;

//summary of CardData objects
[CreateAssetMenu(menuName = "Card Collection")]
public class CardCollection : ScriptableObject
{
    [field: SerializeField] public List<CardData> CardsInCollection {get; private set;}

    //optional other methods

    public void RemoveCardFromCollection(CardData card)
    {
        if (CardsInCollection.Contains(card))
        {
            CardsInCollection.Remove(card);
        } else
        {
            Debug.LogWarning("CardData not present");
        }
    }

    public void AddCardToCollection(CardData card)
    {
        CardsInCollection.Add(card);
    }
    



}
