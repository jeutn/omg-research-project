using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set;} //singleton

    //reference to the deck through cardcollection
    [SerializeField] private CardCollection _cardDataCollection; //represents card data to be instantiated - maybe change name 
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Canvas _cardCanvas;
    [SerializeField] private int halfSunCount = 40;

    //represent instantiated cards
    private List<Card> _deckPile = new();
    private List<Card> _discardPile = new();

    private void Awake()
    {
        //instance declaration
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InstantiateDeck();
    }

    private void InstantiateDeck()
    {
        for (int i = 0; i < _cardDataCollection.CardsInCollection.Count; i++)
        {
            Card card = Instantiate(_cardPrefab, _cardCanvas.transform); //instantiates cardprefab as child of card canvas 
            card.cardLocation = CardLocation.Deck;
            card.SetUp(_cardDataCollection.CardsInCollection[i]); //check this
            _deckPile.Add(card); //all cards in deck at the start, none in player hand or discard
            card.gameObject.SetActive(false);
            
        }
        ShuffleDeck();
    }

    //fisher yates
    public void ShuffleDeck() => GameUtils.FisherYates(_deckPile); //switched to shuffle in library method 
    /*{
        for (int i = _deckPile.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i+1);
            var temp = _deckPile[i];
            _deckPile[i] = _deckPile[j];
            _deckPile[j]= temp;
        }
        
    }*/

    //draws one card at a time 
    public Card DrawCard()
    {
        if (_deckPile.Count <= 0) //no more cards in deck 
        {
            _deckPile.AddRange(_discardPile);
            _discardPile.Clear();
            ShuffleDeck();
        }

        Card card = _deckPile[0];
        _deckPile.RemoveAt(0);

        card.gameObject.SetActive(true);
        return card;
    }

    //draws multiple cards into player hand 
    public void DrawHand(Player player, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Card card = DrawCard();
            if (card == null) break;

            card.owner = player;
            card.cardLocation = CardLocation.Hand;
            player.playerHand.Add(card);

            // parent to correct hand area
            Transform handArea = UIManager.Instance.GetHandArea(player);
            if (handArea != null)
                card.transform.SetParent(handArea, false);
            
        }

    }

    //wrapper for button
    public void DrawHandButton()
    {
        //DrawHand(GameManager.Instance.'CURRENTPLAYER', 2);
    }

    public void DrawPrepCardsButton() //should just use drawhand, and create variables for how many cards in phasemanager or gamemanager 
    {
        DrawHand(GameManager.Instance.currentPlayer, 2);
        PhaseManager.Instance.PlayerTurnEnd();
    }

    public void DiscardCard(Card card)
    {
            _discardPile.Add(card);
            card.cardLocation = CardLocation.Discard;
            card.gameObject.SetActive(false);
    }

}
