using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    //phase conclusions driven by player turns, create a playerturnend() method 
    // handle everything based on turn state 
    public int CoinToVPConversion = 5;

    public static PhaseManager Instance { get; private set; }

    private List<Player> _turnOrder = new();
    private int _currentPlayerIndex = 0; 
    public Player CurrentPlayer => _turnOrder[_currentPlayerIndex];

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //setup player turn order 
    public void SetTurnOrder()
    {
        _turnOrder.Clear();
        _turnOrder.Add(GameManager.Instance.player1);
        _turnOrder.Add(GameManager.Instance.player2);
        _currentPlayerIndex = 0;
        UIManager.Instance.UpdateTurnBanner(CurrentPlayer);
    }

    //PHASE 1: PREPARATION - MARKET OPEN
    public void RunPreparation()
    {
        //draw extra cards according to any market offices
        Deck.Instance.DrawHand(GameManager.Instance.player1, 
                                MarketOfficeManager.Instance.ExtraDrawCount(GameManager.Instance.player1));
        Deck.Instance.DrawHand(GameManager.Instance.player2, 
                        MarketOfficeManager.Instance.ExtraDrawCount(GameManager.Instance.player2));

        MarketManager.Instance.DrawUntilSun();
        
    }

    //PHASE 2: PLANNING - PLAYER DECISIONS
    public void RunPlanning()
    {
        //player 1 - choose worker mode (need select/deselect cards, goods: 0 at top, coins)
        UIManager.Instance.ShowWorkerSelection();
        
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
    }

    //PLAYER TURN END  
    public void PlayerTurnEnd()
    {
        _currentPlayerIndex++;

        if (_currentPlayerIndex >= _turnOrder.Count)
        {
            _currentPlayerIndex = 0;
            GameManager.Instance.NextPhase();
            return; 
        }

        UIManager.Instance.UpdateTurnBanner(CurrentPlayer);

        // PHASE-SPECIFIC UI 
        if (GameManager.Instance.currentPhase == GamePhase.Setup)
        {
            UIManager.Instance.ShowSetupPanel(CurrentPlayer);
        }
        // GLOBAL RULE: only current player sees their hand
        if (GameManager.Instance.currentPhase != GamePhase.Planning)
        {
            UIManager.Instance.SetHandFaceDown(GameManager.Instance.player1, true);
            UIManager.Instance.SetHandFaceDown(GameManager.Instance.player2, true);
            UIManager.Instance.SetHandFaceDown(CurrentPlayer, false);
            
        }

        
    }

    //ROUND END 
    public void RoundEnd()
    {
        MarketManager.Instance.ClearMarket();
        ResetPlayer(GameManager.Instance.player1);
        ResetPlayer(GameManager.Instance.player2);
        GameManager.Instance.NextPhase();
    }

    public void ResetPlayer(Player player)
    {
        //resets everything temporary 
        player.productionBuilding = null;
        player.workerMode = WorkerMode.Default;
        player.queuedBuilding = null;

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

    //ROUND OPEN HELPER METHODS
    public void DrawRoundOpenButton()
    {
        //Player player = GameManager.Instance.currentPlayer;
        Deck.Instance.DrawHand(GameManager.Instance.player1, 2);
        UIManager.Instance.SetHandFaceDown(GameManager.Instance.player1, true);
        Deck.Instance.DrawHand(GameManager.Instance.player2, 2);
        UIManager.Instance.SetHandFaceDown(GameManager.Instance.player2, true);
        
        UIManager.Instance.ShowPanel(UIManager.Instance.sunrisePanel);
        GameManager.Instance.NextPhase();
    }

}
