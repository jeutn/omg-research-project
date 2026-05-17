using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance;
    public ProdCardData[] charburners;
    private int startingGoodsAmt = 7;
    private int _charburnerIndex = 0;

    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Canvas _cardCanvas;
    [SerializeField] private Transform player1BuildingSite;
    [SerializeField] private Transform player2BuildingSite;

    private void Awake()
    {
        ShuffleCharburners();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void ShuffleCharburners() => GameUtils.FisherYates(charburners); //changed to using shuffle in library method 
    

    //button #1 - called by set up player x button
    public void DealCharburner()
    {
        Debug.Log($"Charburner index: {_charburnerIndex}, array length: {charburners.Length}");
    
        if (charburners == null || charburners.Length == 0)
        {
            Debug.LogError("No charburners assigned in inspector");
            return;
        }
        //deal charburners into building site 
        Player player = GameManager.Instance.currentPlayer;
        Transform buildingSite = player == GameManager.Instance.player1 
        ? player1BuildingSite 
        : player2BuildingSite;

        Card charburner = InstantiateCharburner(charburners[_charburnerIndex++], buildingSite);
        player.buildingSite.Add(charburner);

        if (charburner.cardData is ProdCardData prodCardData)
        {
            player.goodsInventory[prodCardData.prodOutput] += startingGoodsAmt;
            player.coins += ResourceCoinValues.Value[prodCardData.prodOutput] * startingGoodsAmt;
        }

        Debug.Log("Charburner dealt to player: " + player.playerID);
        // show the next button
        UIManager.Instance.ShowDrawCardsButton();
    }

    public void DrawStartingHand()
    {
        Player player = GameManager.Instance.currentPlayer;
        Deck.Instance.DrawHand(player, 5);

        // show finish setup button
        UIManager.Instance.ShowFinishSetupButton();
    }

    public void FinishSetup()
    {
        PhaseManager.Instance.PlayerTurnEnd();
    }

    private Card InstantiateCharburner(CardData cardData, Transform parent)
    {
            Card card = Instantiate(_cardPrefab, parent); //instantiates cardprefab as child of card canvas 
            card.SetUp(cardData); 
            card.cardLocation = CardLocation.BuildingSite;
            return card;
 

    }



    

}
