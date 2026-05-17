using Unity.VisualScripting;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    [Header("Panels")]
    public GameObject startMenuPanel;
    public GameObject gameSetupPanel;
    public GameObject preparationPanel;
    public GameObject sunrisePanel;
    public GameObject planningPanel;
    public GameObject sunsetPanel;
    public GameObject productionPanel;
    public GameObject roundEndPanel;
    public GameObject gameEndPanel;

    private GameObject[] _allPanels;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI turnBannerText;

    [SerializeField] private TextMeshProUGUI phaseBannerText;
    [SerializeField] private TextMeshProUGUI halfSunCountText;
    [SerializeField] private TextMeshProUGUI winnerText;

	[Header("Setup Buttons")]
	[SerializeField] private GameObject setupPlayerButton;
	[SerializeField] private GameObject drawCardsButton;
	[SerializeField] private GameObject finishSetupButton;


    [SerializeField] private GameObject sharedBoard;
    [SerializeField] private Transform player1Hand;
    [SerializeField] private Transform player2Hand;

	private GameObject _currentPanel;

    private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);

		_allPanels = new GameObject[]
		{
			startMenuPanel, gameSetupPanel, preparationPanel,
			sunrisePanel, planningPanel, sunsetPanel,
			productionPanel, roundEndPanel, gameEndPanel
		};
	}

	private void Start()
	{
		foreach (GameObject p in _allPanels)
        	p.SetActive(false);
		
		_currentPanel = startMenuPanel;
		startMenuPanel.SetActive(true); // only show start menu on launch
	}

	//SETUP MENUS

    public void ShowPanel(GameObject panel)
    {
		if (_currentPanel != null) _currentPanel.SetActive(false);
		_currentPanel = panel;
		panel.SetActive(true);
    }

	public void ShowSetupPanel(Player player)
	{
		// reset buttons
		setupPlayerButton.SetActive(true);
		drawCardsButton.SetActive(false);
		finishSetupButton.SetActive(false);

		UpdateTurnBanner(player);
	}

	public void ShowDrawCardsButton()
	{
		setupPlayerButton.SetActive(false);
    	drawCardsButton.SetActive(true);
	}

	public void ShowFinishSetupButton()
	{
		drawCardsButton.SetActive(false);
		finishSetupButton.SetActive(true);
	}

	//PLAYER HAND AREA 
	public Transform GetHandArea(Player player)
	{
		    return player == GameManager.Instance.player1 ? player1Hand : player2Hand;
	}

    public void UpdateTurnBanner(Player player)
    {
        turnBannerText.text = $"Player {player.playerID}'s turn";
    }

    public void UpdatePhaseBanner()
    {
        phaseBannerText.text = $"Current phase: {GameManager.Instance.currentPhase}";
    }

    /*public void UpdateUI(GamePhase phase, PlayerTurn turn)
        {
            UpdatePanels(turn);
            UpdatePhaseUI(phase);
        }

    private void UpdatePanels(PlayerTurn turn)
        {
            player1Hand.SetActive(turn == PlayerTurn.player1);
            player2Hand.SetActive(turn == PlayerTurn.player2); //either use PlayerTurn enums or use the currentPlayerIndex system...
        }*/

    private void UpdatePhaseUI(GamePhase phase)
        {
            // show/hide banner with phase name + pass button to pass turn 
        }

	//method to refresh buildingUI stats for each building in building site
	

}		
