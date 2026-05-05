using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Production Card")]
public class ProdCardData : CardData
{
    public ResourceType prodOutput;
    public int goodsvalue;

    [System.Serializable]
    public struct ResourceAmts
    {
        public ResourceType type;
        public int amount;
    }
    public ResourceAmts[] productionInput; 
    public ResourceType[] productionChain;







}
