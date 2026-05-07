using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public GamePhase currentPhase {get; private set;}
    public PlayerTurn currentTurn {get; private set;}
    public Player player1;
    public Player player2;
    // public List<Player> players; - Maybe add players to a list instead of having only 2 

    // public Player CurrentPlayer; - Maybe use a currentPlayer tracker instead of doing player1, then player2? 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        currentPhase = GamePhase.Setup;
        currentTurn = PlayerTurn.player1;
        GameSetup.Instance.StartGameSetup();
        
    }

    public void NextPhase()
    {
        //move to next phase, currentphase ++
        currentPhase++;

        //update UI phase banner to show next phase title 

        switch (currentPhase)
        {
            case GamePhase.MarketOpen:
                MarketManager.Instance.DrawUntilSun();
                break;

            case GamePhase.Planning:
                //planning methods for player choosing building, worker mode, lock in decisions for this round 
                break;

            case GamePhase.MarketClose:
                break;

            case GamePhase.Production:
                PhaseManager.Instance.RunProduction();
                break;

            case GamePhase.Cleanup:
                PhaseManager.Instance.CleanUp(); //do i really need a phase manager, i can just call marketmanager.instance.clearmarket() here...
                //banner updates round number++
                break; 

            case GamePhase.GameEnd:
                GameEnd();
                break;


        }




    }

    public void PassTurn()
    {
        //player passes turn 

    }

    public void GameEnd()
    {
        //clear everything, reset deck
        //method for tallying winner points 
    }




}
