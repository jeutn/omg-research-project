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
        for (int i = 0; i < _playerDeck.CardsInCollection.Count; i++)
        {
            Card card = Instantiate(_cardPrefab, _cardCanvas.transform); //instantiates cardprefab as child of card canvas 
            card.SetUp(_playerDeck.CardsInCollection[i]); //check this
            _deckPile.Add(card); //all cards in deck at the start, none in player hand or discard
            card.gameObject.SetActive(false);
            
        }
        ShuffleDeck();
        ApplyHalfSun();
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

    public void DrawHand(Player player, int amount)
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

            _deckPile.RemoveAt(0);
            _deckPile[0].owner = player;
            _deckPile[0].gameObject.SetActive(true);
            player.playerHand.Add(_deckPile[0]);
        }

    }

    public void DiscardCard(Player player, Card card)
    {
        if (player.playerHand.Contains(card))
        {
            player.playerHand.Remove(card);
            _discardPile.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    private void ApplyHalfSun()
{
    for (int i = 0; i < _deckPile.Count; i++)
    {
        _deckPile[i].halfSun = i < halfSunCount;
    }
}
}
