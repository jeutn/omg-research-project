using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public static WorkerManager Instance {get; private set;}
    public int efficientGoods = 2;
    public int sloppyGoods = 1;
    private Card _selectedBuilding; 

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

        bool bothChosen = 
            GameManager.Instance.player1.workerMode != WorkerMode.Default &&
            GameManager.Instance.player2.workerMode != WorkerMode.Default;

        if (bothChosen)
        {
            // both done, go straight to building selection for player 1
            UIManager.Instance.HideWorkerSelection();
            PhaseManager.Instance.SetTurnOrder();
            UIManager.Instance.UpdateTurnBanner(GameManager.Instance.currentPlayer);
            UIManager.Instance.ShowBuildingSelection();
            ShowAvailableBuildings(GameManager.Instance.currentPlayer);
        }
        else
        {
            // player 1 done, advance to player 2 for worker selection
            PhaseManager.Instance.PlayerTurnEnd();
        }
    }

    //default to be efficient worker 
    public void DefaultWorkerSelection(Player player)
    {
        OnWorkerSelected(player, WorkerMode.Efficient);
    }

    //CHOOSING A BUILDING FOR PRODUCTION
    //players choose a building by clicking - need to use OnClick 
    private void BeginBuildingSelection()
    {
        PhaseManager.Instance.SetTurnOrder();
        _selectedBuilding = null;
        UIManager.Instance.UpdateTurnBanner(GameManager.Instance.currentPlayer);
        ShowAvailableBuildings(GameManager.Instance.currentPlayer);
    }

    public void ShowAvailableBuildings(Player player)
    {
        foreach (Card building in player.buildingSite)
        {
            building.GetComponent<CardUI>().SetSelectable(true);
        }
    }

    public void ClearAvailableBuildings(Player player)
    {
        foreach (Card building in player.buildingSite)
        {
            building.GetComponent<CardUI>().ClearBorder();
        }
    }
    // called by OnClick on each building card
    public void OnBuildingSelected(Card building)
    {
        
        if (_selectedBuilding != null)
            _selectedBuilding.GetComponent<CardUI>().SetSelected(false);

        _selectedBuilding = building;
        building.GetComponent<CardUI>().SetSelected(true);

        //maybe highlight the building selected (yellow border outline or something)

        // planning turn done for this player
        //PhaseManager.Instance.PlayerTurnEnd();
    }

    // confirm button for production building 
    public void ConfirmBuildingSelection()
    {
        if (_selectedBuilding == null) return;

        Player player = GameManager.Instance.currentPlayer;

        // clear previous production building highlight
        if (player.productionBuilding != null)
            player.productionBuilding.GetComponent<CardUI>().SetAsProductionBuilding(false);

        // set new production building
        player.productionBuilding = _selectedBuilding;
        
        // clear grey borders first
        ClearAvailableBuildings(player);
        
        // then set green — must happen after clear
        player.productionBuilding.GetComponent<CardUI>().SetAsProductionBuilding(true);
        
        _selectedBuilding = null;
        PhaseManager.Instance.PlayerTurnEnd();
    }



}
