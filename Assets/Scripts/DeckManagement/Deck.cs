using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set;} //singleton

    //reference to the deck through cardcollection
    [SerializeField] private CardCollection _playerDeck;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Canvas _cardCanvas;

    //represent instantiated cards
    private List<Card> _deckPile = new();
    private List<Card> _discardPile = new();
    public List<Card> HandCards {get; private set;} = new();

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
        for (int i = 0; i < _playerDeck.CardsInCollection.Count; i++)
        {
            Card card = Instantiate(_cardPrefab, _cardCanvas.transform); //instantiates cardprefab as child of card canvas 
            card.SetUp(_playerDeck.CardsInCollection[i]); //check this
            _deckPile.Add(card); //all cards in deck at the start, none in player hand or discard
            card.gameObject.SetActive(false);
            
        }
        ShuffleDeck();
    }

    //fisher yates
    public void ShuffleDeck()
    {
        for (int i = _deckPile.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i+1);
            var temp = _deckPile[i];
            _deckPile[i] = _deckPile[j];
            _deckPile[j]= temp;
        }
        
    }

    public void DrawHand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_deckPile.Count <= 0)
            {
                _discardPile = _deckPile;
                _discardPile.Clear();
                ShuffleDeck();
            }
            HandCards.Add(_deckPile[0]);
            _deckPile[0].gameObject.SetActive(true);
            _deckPile.RemoveAt(0);

            //edge case, if all drawn cards are in player hand 
            if (_deckPile.Count > 0)
            {
                HandCards.Add(_deckPile[0]);
                _deckPile[0].gameObject.SetActive(true);
                _deckPile.RemoveAt(0);
            }
        }



    }

    public void DiscardCard(Card card)
    {
        if (HandCards.Contains(card))
        {
            HandCards.Remove(card);
            _discardPile.Remove(card);
            card.gameObject.SetActive(false);
        }
    }
}
