using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardMovement))] //handles everything perceived with card movement
[RequireComponent(typeof(CardUI))] //will automatically attach CardUI script to every card object

public class Card : MonoBehaviour
{
    public Player owner;
    
    public CardData cardData;
    public CardLocation cardLocation;

    public void SetUp(CardData data)
    {
        cardData = data;
        GetComponent<CardUI>().Setup(this);
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (cardLocation == CardLocation.Hand || cardLocation == CardLocation.Market)
            {
                if (GameManager.Instance.currentPlayer.selectedResources.Contains(this))
                    PhaseManager.Instance.DeselectProductionCard(this);
                else
                    PhaseManager.Instance.SelectProductionCard(this);
            }
            else if (cardLocation == CardLocation.BuildingSite)
            {
                WorkerManager.Instance.OnBuildingSelected(this);
            }
        });
    }
    

}

