using System;
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
    [SerializeField] private int halfSunCount = 20;

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
        ApplyHalfSun();
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

    public Card DrawCard()
    {
        if (_deckPile.Count == 0)
        {
            return null;
        }

        Card card = _deckPile[0];
        _deckPile.RemoveAt(0);

        card.gameObject.SetActive(true);
        return card;
    }

    public void DrawHand(Player player, int amount) //discardpile only gets put back into main deck in this method...need to separate out
    {
        for (int i = 0; i < amount; i++)
        {
            if (_deckPile.Count <= 0)
            {
                if (_discardPile.Count <= 0) break; //no cards anywhere, stop drawing
                {
                    _deckPile.AddRange(_discardPile);
                    _discardPile.Clear();
                    ShuffleDeck();
                }
            }

            Card card = _deckPile[0];
            _deckPile.RemoveAt(0);

            card.owner = player;
            card.gameObject.SetActive(true);
            player.playerHand.Add(card);
            card.cardLocation = CardLocation.Hand;
        }

    }

    public void DiscardCard(Player player, Card card)
    {
        if (player.playerHand.Contains(card))
        {
            player.playerHand.Remove(card);
            _discardPile.Add(card);
            card.cardLocation = CardLocation.Discard;
            card.gameObject.SetActive(false);
        }
    }

    private void ApplyHalfSun() //justify 
{
    for (int i = 0; i < _deckPile.Count; i++)
    {
        _deckPile[i].halfSun = i < halfSunCount;
    }
}
}
