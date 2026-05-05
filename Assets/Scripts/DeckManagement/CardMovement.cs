using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Drawing;

//handles drag and drop of cards
public class CardMovement : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private bool _isBeingDragged;
    private Canvas _cardCanvas; //need to get this at runtime, assigning in inspector won't work
    private RectTransform _rectTransform;
    private Card _card;
    private readonly string CANVAS_TAG = "CardCanvas";

    private void Start()
    {
        _cardCanvas = GameObject.FindGameObjectWithTag(CANVAS_TAG).GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _card = GetComponent<Card>();
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        _isBeingDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += (eventData.delta / _cardCanvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isBeingDragged = false;
        Deck.Instance.DiscardCard(_card.owner, _card);
    }
}
