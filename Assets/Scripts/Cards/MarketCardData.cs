using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Market Office Card")]
public class MarketCardData : CardData
{
    public ResourceType bonusResource;
    public CardEffectType cardEffectType;
    public int extraCards = 1;

}
