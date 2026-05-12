using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    //phase conclusions driven by player turns, create a playerturnend() method 
    // handle everything based on turn state 
    public int CoinToVPConversion = 5;

    public static PhaseManager Instance { get; private set; }

    //PHASE 1: PREPARATION - MARKET OPEN
    public void RunPreparation()
    {
        //draw extra cards according to any market offices
        Deck.Instance.DrawHand(GameManager.Instance.player1, 
                                MarketOfficeManager.Instance.ExtraDrawCount(GameManager.Instance.player1));
        Deck.Instance.DrawHand(GameManager.Instance.player2, 
                        MarketOfficeManager.Instance.ExtraDrawCount(GameManager.Instance.player2));
        
    }

    //PHASE 2: PLANNING - PLAYER DECISIONS
    public void RunPlanning()
    {
        
    }

    //PHASE 3: RESOURCE FINALISATION - MARKET CLOSE
    public void RunFinalisation()
    {
        
    }

    //PHASE 4: RESOLVE PRODUCTION 
    public void RunProduction()
    {
        //run production logic
        ProductionManager.Instance.ResolveProduction(GameManager.Instance.player1); //p1 and p2 will show at the same time with this code...do i want a delay, also would it be better to use a currentPlayer index that cycles through rather than coding for p1/p2 each time
        ProductionManager.Instance.ResolveProduction(GameManager.Instance.player2); //can move to phasemanager and call runProduction
        ProductionManager.Instance.EndProduction(GameManager.Instance.player1); //cleanup phase 4
        ProductionManager.Instance.EndProduction(GameManager.Instance.player2); //cleanup phase 4

    }

    //CLEAN UP
    public void CleanUp()
    {
        MarketManager.Instance.ClearMarket();
        GameManager.Instance.NextPhase();
        //RESET WORKER MODE TO DEFAULT 
        
    }

    //PLAYER TURN END  
    public void PlayerTurnEnd()
    {
        
    }

    //ENDGAME METHODS
    public void GameEnd()
    {
        //clear everything, reset deck
        //method for tallying winner points 
    }
    private int CoinsToVP(Player player)
    {
        return player.coins % CoinToVPConversion;
    }

    //in case of a tie between players 
    private Player TieBreaker(Player player1, Player player2)
    {
        if (player1.coins == player2.coins)
        {
            Debug.Log("Tie - both players win");
            return null;
        }
        return player1.coins > player2.coins ? player1 : player2;
    }

}
