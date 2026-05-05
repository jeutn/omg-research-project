using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public GamePhase currentPhase {get; private set;}
    public PlayerTurn currentTurn {get; private set;}

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
        
    }

    public void NextPhase()
    {
        //move to next phase, currentphase ++
    }

    public void PassTurn()
    {
        //player passes turn 
    }

    private void ResolveProduction()
    {
        //production logic - should this go in another script 
    }




}
