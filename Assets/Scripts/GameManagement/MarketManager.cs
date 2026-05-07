using UnityEngine;
using System.Collections.Generic;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance { get; private set; }
    public List<Card> marketDisplay = new();
    private int maxHalfSuns = 2;

    public void DrawUntilSun()
    {
        int halfSunsCount = 0;
        //. draw until maxHalfSuns are met 
        while (halfSunsCount < maxHalfSuns)
        {
            Card drawn = Deck.Instance.DrawCard();
            marketDisplay.Add(drawn);

            if (drawn.halfSun)
            {
                halfSunsCount++;
            }
            
        }
    }

    public bool ResourceAvailable()
    {
        //checks if resource is available
        return true;
    }

    public void ClearMarket()
    {
        //clears the market display 
    }
}
