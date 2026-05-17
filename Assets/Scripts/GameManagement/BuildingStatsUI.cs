using UnityEngine;
using TMPro;

public class BuildingStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingStats;
    private Card _building;

    public void Setup(Card building)
    {
        _building = building;
        Refresh();
    }

    public void Refresh()
    {
        if (_building.cardData is not ProdCardData prodData) return;
        int count = _building.owner.goodsInventory[prodData.prodOutput];
        buildingStats.text = $"{prodData.prodOutput}: {count}";
    }


}
