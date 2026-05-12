using UnityEngine;

public class MarketOfficeManager : MonoBehaviour
{
    public static MarketOfficeManager Instance {get; private set;}

    //player has multiple market offices that allow for extra dealt cards 
    public int ExtraDrawCount(Player player)
    {
        int total = 0;
        foreach(Card card in player.buildingSite)
        {
            if (card.cardData.cardType != CardType.marketOffice) continue;
            if (card.cardData is not MarketCardData marketCardData) continue;
            if (marketCardData.cardEffectType != CardEffectType.DrawCard) continue;

            total += marketCardData.extraCards;
        }
        return total;
    }
}
