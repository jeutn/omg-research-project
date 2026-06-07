using UnityEngine;
using System.Collections.Generic;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance { get; private set; }
    public List<Card> marketDisplay = new();
    private int _halfSunsCount = 0;
    private int _maxHalfSuns = 2;
    [SerializeField] private Transform marketArea;

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
        card.transform.SetParent(marketArea, false);

        if (card.cardData.halfSun)
        {
            _halfSunsCount++;
            if (_halfSunsCount >= _maxHalfSuns)
            {
                _halfSunsCount = 0;
                GameManager.Instance.NextPhase();
                return; // stop here when 2 half suns appear, move to next phase 
            }
        }

    }

    public void MarketFullSun()
    {
        _halfSunsCount = 0; //reset for next phase 
        GameManager.Instance.NextPhase();
        if (GameManager.Instance.currentPhase == GamePhase.MarketOpen)
        {
            UIManager.Instance.ShowPanel(UIManager.Instance.planningPanel);
        }
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
