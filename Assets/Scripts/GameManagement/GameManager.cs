using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public GamePhase currentPhase {get; private set;}
    public Player currentPlayer => PhaseManager.Instance.CurrentPlayer;
    [SerializeField] public Player player1;
    [SerializeField] public Player player2;

    private int currentRound = 0;
    private int numOfRounds = 7; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGame()
    {
        currentPhase = GamePhase.Setup;
        PhaseManager.Instance.SetTurnOrder();
        
        UIManager.Instance.UpdateTurnBanner(currentPlayer);
        UIManager.Instance.ShowPanel(UIManager.Instance.gameSetupPanel);
        UIManager.Instance.ShowSetupPanel(currentPlayer);
        
    }

    public void NextPhase()
    {
        //move to next phase, currentphase ++
        currentPhase++;

        //update UI phase banner to show next phase title 

        switch (currentPhase)
        {
            case GamePhase.RoundOpen:
                //draw 2 cards - PREP PANEL DOESNT EXIST RN 
                PhaseManager.Instance.SetTurnOrder();
                UIManager.Instance.ShowPanel(UIManager.Instance.preparationPanel);
                UIManager.Instance.UpdateTurnBanner(currentPlayer);
                break;

            case GamePhase.MarketOpen:
                MarketManager.Instance.DrawUntilSun();
                break;

            case GamePhase.Planning:
                //planning methods for player choosing building, worker mode, lock in decisions for this round 
                break;

            case GamePhase.MarketClose:
                MarketManager.Instance.DrawUntilSun();
                break;

            case GamePhase.Production:
                PhaseManager.Instance.RunProduction();
                break;

            case GamePhase.RoundEnd:
                PhaseManager.Instance.RoundEnd();

                //banner updates round number++
                break; 

            case GamePhase.GameEnd:
                PhaseManager.Instance.GameEnd();
                break;


        }

    }

    public void AdvanceRound()
    {
        currentRound++;
        if (currentRound >= numOfRounds)
        {
            //game end panel
            currentPhase = GamePhase.GameEnd;
            PhaseManager.Instance.GameEnd();
            return;
        }
        currentPhase = GamePhase.RoundOpen;
        NextPhase();

    }

    public void PassTurn()
    {
        //player passes turn button 

    }




}
