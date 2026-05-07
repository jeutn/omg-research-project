using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    [SerializeField] private GameObject sharedBoard;
    [SerializeField] private GameObject player1Panel;
    [SerializeField] private GameObject player2Panel;
    [SerializeField] private GameObject phaseBanner;

    private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

    public void UpdateUI(GamePhase phase, PlayerTurn turn)
        {
            UpdatePanels(turn);
            UpdatePhaseUI(phase);
        }

    private void UpdatePanels(PlayerTurn turn)
        {
            player1Panel.SetActive(turn == PlayerTurn.player1);
            player2Panel.SetActive(turn == PlayerTurn.player2); //either use PlayerTurn enums or use the currentPlayerIndex system...
        }

    private void UpdatePhaseUI(GamePhase phase)
        {
            // show/hide banner with phase name + pass button to pass turn 
        }


}
