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
    [SerializeField] public TextMeshProUGUI turnBannerText;

    [SerializeField] private TextMeshProUGUI phaseBannerText;
	[SerializeField] private GameObject _instructionBanner;
	[SerializeField] public TextMeshProUGUI instructionBannerText;
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

	//STARTMENUPANEL
	
	//ROUND CUSTOMIER
	[SerializeField] private TextMeshProUGUI roundCountText;
	private int _selectedRounds = 7;
	private int _minRounds = 4;
	private int _maxRounds = 12;

	//PLANNING DECISIONS
	[Header("Worker UI")]
	[SerializeField] private GameObject workerSelectionObject;
	[SerializeField] private TextMeshProUGUI worker1BannerText;
	[SerializeField] private TextMeshProUGUI worker2BannerText;
	[SerializeField] private GameObject buildingSelectionObject;
	[SerializeField] private GameObject confirmBuildingButton;


	//PLAYER STATS
	[Header("Player Stats")]
	[SerializeField] private TextMeshProUGUI player1CoinsText;
	[SerializeField] private TextMeshProUGUI player2CoinsText;

	public void RefreshCoins()
	{
		player1CoinsText.text = $"P1 Coins: {GameManager.Instance.player1.coins}";
		player2CoinsText.text = $"P2 Coins: {GameManager.Instance.player2.coins}";
	}

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

		_instructionBanner.SetActive(false);
	}

	private void Start()
	{
		foreach (GameObject p in _allPanels)
        	p.SetActive(false);
		
		_currentPanel = startMenuPanel;
		startMenuPanel.SetActive(true); // only show start menu on launch

		//set worker banners 
		worker1BannerText.text = "P1 Worker: -";
		worker2BannerText.text = "P2 Worker: -";

		//disable instruction banner
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

		// hide hands
    	SetHandFaceDown(GameManager.Instance.player1, true);
		SetHandFaceDown(GameManager.Instance.player2, true);

    	// only show current player hand
    	SetHandFaceDown(player, false);

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

	//ROUND CUSTOMISER
	public void AddRounds()
	{
		if (_selectedRounds < _maxRounds)
		{
			_selectedRounds++;
			roundCountText.text = $"Rounds: {_selectedRounds}";
		}
	}

	public void SubtractRounds()
	{
		if (_selectedRounds > _minRounds)
		{
			_selectedRounds--;
			roundCountText.text = $"Rounds: {_selectedRounds}";
		}
	}

	public int GetSelectedRounds() => _selectedRounds;

	//CARD BACKING
	public void SetHandFaceDown(Player player, bool faceDown)
	{
		foreach (Card card in player.playerHand)
		{
			card.GetComponent<CardUI>().SetFaceDown(faceDown);
		}
	}

	//WORKER UI METHODS

	public void ShowWorkerSelection()
	{
		workerSelectionObject.SetActive(true);
		buildingSelectionObject.SetActive(false);
		confirmBuildingButton.SetActive(false);
	}

	public void HideWorkerSelection()
	{
		workerSelectionObject.SetActive(false);
	}

	public void UpdateWorkerBanner(Player player)
	{
		if (player == GameManager.Instance.player1)
			worker1BannerText.text = $"P1 Worker: {player.workerMode}";
		else
			worker2BannerText.text = $"P2 Worker: {player.workerMode}";

		//RESET
		//worker1BannerText.text = "P1 Worker: -";
		//worker2BannerText.text = "P2 Worker: -";
	}

	public void ShowBuildingSelection()
	{
		workerSelectionObject.SetActive(false);
		buildingSelectionObject.SetActive(true);
		confirmBuildingButton.SetActive(true); // explicitly show every time
		_instructionBanner.SetActive(true);
		instructionBannerText.text = $"Player {GameManager.Instance.currentPlayer.playerID}: select a building for production";
	}

    private void UpdatePhaseUI(GamePhase phase)
        {
            // show/hide banner with phase name + pass button to pass turn 
        }

	//method to refresh buildingUI stats for each building in building site

	//BUILDING GOODS COUNTER

	public void RefreshBuildingSite(Player player)
	{
		foreach (Card building in player.buildingSite)
		{
			if (building.cardData is not ProdCardData prodData) continue;
			
			ResourceType resource = prodData.prodOutput;
			int count = player.goodsInventory.ContainsKey(resource) 
				? player.goodsInventory[resource] 
				: 0;

			building.GetComponent<CardUI>().UpdateGoodsCounter(count, resource);
		}
	}
	

}		
