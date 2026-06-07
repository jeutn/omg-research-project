using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance {get; private set;}
    public int efficientGoods = 2;
    public int sloppyGoods = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //method to show worker selection in UIManager, players need to click on which worker they want, then call OnBuildingSelected to attach to specific building 

    //method if default -> choose efficient worker 
	// called by Efficient button
    public void SelectEfficient()
    {
        Player player = GameManager.Instance.currentPlayer;
        OnWorkerSelected(player, WorkerMode.Efficient);
    }

    // called by Sloppy button
    public void SelectSloppy()
    {
        Player player = GameManager.Instance.currentPlayer;
        OnWorkerSelected(player, WorkerMode.Sloppy);
    }

    private void OnWorkerSelected(Player player, WorkerMode mode)
    {
        player.workerMode = mode;
        UIManager.Instance.UpdateWorkerBanner(player);

        // check if both players have chosen
        if (GameManager.Instance.player1.workerMode != WorkerMode.Default &&
            GameManager.Instance.player2.workerMode != WorkerMode.Default)
        {
            // both done — hide popup and begin building selection
            UIManager.Instance.HideWorkerSelection();
            //BeginBuildingSelection();
        }
        else
        {
            // advance to next player for worker selection
            PhaseManager.Instance.PlayerTurnEnd();
        }
    }

    //default to be efficient worker 
    public void DefaultWorkerSelection(Player player)
    {
        OnWorkerSelected(player, WorkerMode.Efficient);
    }



    //players choose a building by clicking - need to use OnClick 
    private void OnBuildingSelected(Player player, Card building)
    {
        player.queuedBuilding = building;
        //maybe highlight the building selected (yellow border outline or something)

        // planning turn done for this player
        //PhaseManager.Instance.PlayerTurnEnd();
    }




}
