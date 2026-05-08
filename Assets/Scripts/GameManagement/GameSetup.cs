using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance;
    public ProdCardData[] charburners; //need to add players + gamesetup to canvas, then will show in inspector 
    private int startingGoodsAmt = 7;
    private int _charburnerIndex = 0;

    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Canvas _cardCanvas;

    public void StartGameSetup()
    {
        ShuffleCharburners();
        SetupPlayer(GameManager.Instance.player1);
        SetupPlayer(GameManager.Instance.player2);
        Debug.Log("Game setup done");
    }

    private void SetupPlayer(Player player) //DOESNT SHOW IN UI JUST YET
    {
        //1. add charburner
        Card charburner = GetCharburner();
        player.buildingSite.Add(charburner);

        //2. receive 7 goods to charburner and 3. resources to coins

        // player.goodsInventory[charburner.cardData.prodOutput] += startingGoodsAmt; // maybe have output connected to production building type, but each prod card should have the resource output as a property, can just look up type anyway 
        if (charburner.cardData is ProdCardData prodData)
        {
            player.goodsInventory[prodData.prodOutput] += startingGoodsAmt;
            player.coins += ResourceCoinValues.Value[prodData.prodOutput] * startingGoodsAmt;
        }
        //will have to do this for every production building...need to find another way...INTERFACES? 

        //3. resources to coins
        //player.coins += ResourceCoinValues.Value[ResourceType.coal] * startingGoodsAmt;

        //3. draw 5 cards
        Deck.Instance.DrawHand(player, 5); 
        
    }

    private void ShuffleCharburners() //same as shuffle in deck, but charburners arent apart of main deck...
    {
        for (int i = charburners.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = charburners[i];
            charburners[i] = charburners[j];
            charburners[j]= temp;
        }
    }

    private Card GetCharburner()
    {
        return InstantiateCharburner(charburners[_charburnerIndex++]);
    }

    private Card InstantiateCharburner(CardData cardData)
    {
            Card card = Instantiate(_cardPrefab, _cardCanvas.transform); //instantiates cardprefab as child of card canvas 
            card.SetUp(cardData); 
            return card;
 

    }



    

}
