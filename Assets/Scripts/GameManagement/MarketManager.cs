using UnityEngine;
using System.Collections.Generic;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance { get; private set; }
    public List<Card> marketDisplay = new();
    private int _halfSunsCount = 0;
    private int _maxHalfSuns = 2;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DrawUntilSun()
    {
        Card card = Deck.Instance.DrawCard();
        if (card == null) return;
        card.cardLocation = CardLocation.Market;
        marketDisplay.Add(card);

        //. draw until maxHalfSuns are met 
        if (card.cardData.halfSun)
        {
            _halfSunsCount++;
            //UIManager.Instance.UpdateHalfSunCount(_halfSunCount);
            if (_halfSunsCount >= _maxHalfSuns) MarketFullSun();
        }

    }

    public void MarketFullSun()
    {
        _halfSunsCount = 0; //reset for next phase 
        GameManager.Instance.NextPhase();
    }

    public void ClearMarket()
    {
        //clears the market display 
        foreach (Card card in marketDisplay)
        {
            card.cardLocation = CardLocation.Discard;
            Deck.Instance.DiscardCard(card);
        }

        marketDisplay.Clear(); 
    }
}
