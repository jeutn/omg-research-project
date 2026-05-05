using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;
using UnityEditorInternal;
using Mono.Cecil;
using UnityEditor;

//update UI visuals as per data 
public class CardUI : MonoBehaviour
{
    private Card _card;

    [Header("Prefab Elements")] //references to objects in the prefab
    [SerializeField] private GameObject productionData;
    [SerializeField] private GameObject marketOfficeData;
    [SerializeField] private Image _cardBG;
    [SerializeField] private Image _inputOneImg;
    [SerializeField] private Image _inputTwoImg;
    [SerializeField] private Image _chainOneImg;
    [SerializeField] private Image _chainTwoImg;
    [SerializeField] private Image _goodsOutput;
    [SerializeField] private Image _halfSun;

    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private TMP_Text _coinsValue;
    [SerializeField] private TMP_Text _vpValue;
    [SerializeField] private TMP_Text _goodsValue;
    [SerializeField] private TMP_Text _inputOneVal;
    [SerializeField] private TMP_Text _inputTwoVal;

    [SerializeField] private Image _marketEffect;

    [Header("Sprite Assets")] //references to art folder in assets 
    [SerializeField] private Sprite charburnerBG;
    [SerializeField] private Sprite clayBG;
    [SerializeField] private Sprite grainBG;
    [SerializeField] private Sprite woodBG;
    [SerializeField] private Sprite woolBG;
    [SerializeField] private Sprite marketBG;

    [SerializeField] private Sprite clay;
    [SerializeField] private Sprite grain;
    [SerializeField] private Sprite wood;
    [SerializeField] private Sprite wool;
    [SerializeField] private Sprite stone;

    [SerializeField] private Sprite barrel;
    [SerializeField] private Sprite bread;
    [SerializeField] private Sprite brick;
    [SerializeField] private Sprite cattle;
    [SerializeField] private Sprite clothes;
    [SerializeField] private Sprite coal;
    [SerializeField] private Sprite fabric;
    [SerializeField] private Sprite flour;
    [SerializeField] private Sprite foodstuff;
    [SerializeField] private Sprite glass;
    [SerializeField] private Sprite iron;
    [SerializeField] private Sprite leather;
    [SerializeField] private Sprite meat;
    [SerializeField] private Sprite planks;
    [SerializeField] private Sprite shoes;
    [SerializeField] private Sprite tools;
    [SerializeField] private Sprite windows;


    [SerializeField] private Sprite halfSunSprite; 
    [SerializeField] private Sprite marketExtraCard;
    
    private void Awake()
    {
        _card = GetComponent<Card>();
        Setup(_card);
        
    }

    private void OnValidate()
    {
        Awake();
    }

    public void Setup(Card card)
    {
        if (card == null){
            Debug.LogWarning("CardData is null");
            return;
        }
        //COMMON UI ATTRIBUTES
        //set background based on resource type
        _cardBG.sprite = GetBackground(card.cardData.rawResource);

        //set UI text
        _cardName.text = card.cardData.cardName;
        _coinsValue.text = card.cardData.costToBuild.ToString();
        _vpValue.text = card.cardData.victoryPoints.ToString();

        _coinsValue.gameObject.SetActive(card.cardData.costToBuild > 0);
        _vpValue.gameObject.SetActive(card.cardData.victoryPoints > 0);

        //shows sun if true
        _halfSun.gameObject.SetActive(card.halfSun);  
        
        //split into production and market card setup
        if (card.cardData is ProdCardData prodCardData)
        {
            productionData.SetActive(true);
            marketOfficeData.SetActive(false);
            SetupProd(prodCardData);

        } else if (card.cardData is MarketCardData marketCardData)
        {
            productionData.SetActive(false);
            marketOfficeData.SetActive(true);
            SetupMarket(marketCardData);
        }
    }

    //setup production card
    private void SetupProd(ProdCardData data)
    {
        _goodsOutput.sprite = GetResource(data.prodOutput);

        _goodsValue.text = data.goodsvalue.ToString();

        //production inputs text + sprites
        if (data.productionInput != null && data.productionInput.Length > 0) //safely check if first element exists 
        {
            _inputOneImg.sprite = GetResource(data.productionInput[0].type);
            _inputOneVal.text = data.productionInput[0].amount.ToString();
        } 

        if (data.productionInput != null && data.productionInput.Length > 1)
        {
            _inputTwoImg.sprite = GetResource(data.productionInput[1].type);
            _inputTwoVal.text = data.productionInput[1].amount.ToString();
        }

        //production chain text + sprites
        if (data.productionChain != null && data.productionChain.Length > 0)
        {
            _chainOneImg.sprite = GetResource(data.productionChain[0]);
        } 

        if (data.productionChain != null && data.productionChain.Length > 1)
        {
            _chainTwoImg.sprite = GetResource(data.productionChain[1]);
        } else
        {
            _chainTwoImg.gameObject.SetActive(false);
        }

        
    }

    //setup market card
    private void SetupMarket(MarketCardData data)
    {
        _marketEffect.gameObject.SetActive(true);

        switch (data.cardEffectType)
        {
            case CardEffectType.AddMarketResource:
                _marketEffect.sprite = GetResource(data.bonusResource);
                break;

            case CardEffectType.DrawCard:
                _marketEffect.sprite = marketExtraCard;
                break;
        }
        
    }


    //gets background based on resource type
    private Sprite GetBackground(ResourceType resourceType)
    {
        switch (resourceType)
        {   
            case ResourceType.none: return charburnerBG;
            case ResourceType.wood: return woodBG;
            case ResourceType.grain: return grainBG;
            case ResourceType.wool: return woolBG;
            case ResourceType.clay: return clayBG;
            case ResourceType.stone: return marketBG;
            default: return null;

        }
    }

    private Sprite GetResource(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.clay: return clay;
            case ResourceType.grain: return grain;
            case ResourceType.stone: return stone;
            case ResourceType.wood: return wood;
            case ResourceType.wool: return wool;

            case ResourceType.barrel: return barrel;
            case ResourceType.bread: return bread;
            case ResourceType.brick: return brick;
            case ResourceType.cattle: return cattle;
            case ResourceType.clothes: return clothes;
            case ResourceType.coal: return coal;
            case ResourceType.fabric: return fabric;
            case ResourceType.flour: return flour;
            case ResourceType.foodstuff: return foodstuff;
            case ResourceType.glass: return glass;
            case ResourceType.iron: return iron;
            case ResourceType.leather: return leather;
            case ResourceType.meat: return meat;
            case ResourceType.planks: return planks;
            case ResourceType.shoes: return shoes;
            case ResourceType.tools: return tools;
            case ResourceType.windows: return windows;
            
            
            default: return null;
        }
        
    }


}

