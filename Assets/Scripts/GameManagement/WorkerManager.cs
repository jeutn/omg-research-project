using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance {get; private set;}

    //method to show worker selection in UIManager, players need to click on which worker they want, then call OnBuildingSelected to attach to specific building 
    public void ShowWorkerSelection(Player player) //ui method
    {
        //while worker popup is active, everything else should be frozen 
    }

    private void OnWorkerSelected(Player player, WorkerMode mode)
    {
        player.workerMode = mode;
        //UI to hide the worker selection popup 

        // now player selects which building to activate - building site now available to interact with
    }





    //players choose a building by clicking - need to use OnClick 
    private void OnBuildingSelected(Player player, Card building)
    {
        player.productionBuilding = building;
        //maybe highlight the building selected (yellow border outline or something)

        // planning turn done for this player
        //PhaseManager.Instance.PlayerTurnEnd();
    }




}
