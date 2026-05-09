using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    //phase conclusions driven by player turns, create a playerturnend() method 
    // handle everything based on turn state 

    public static PhaseManager Instance { get; private set; }

    //PHASE 1: PREPARATION - MARKET OPEN
    public void RunPreparation()
    {
        
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

}
