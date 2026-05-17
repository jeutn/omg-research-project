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
    private Vector2 _originalPosition;
    private Transform _originalParent;
    private readonly string CANVAS_TAG = "CardCanvas";

    private void Start()
    {
        _cardCanvas = GameObject.FindGameObjectWithTag(CANVAS_TAG).GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _card = GetComponent<Card>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;

        // lift card above other cards visually
        transform.SetParent(_cardCanvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += (eventData.delta / _cardCanvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_card.cardLocation == CardLocation.BuildingSite)
        {
            // snap back to building site, just reparent
            transform.SetParent(_originalParent);
            return;
        }

        if (_card.cardLocation == CardLocation.Hand)
        {
            // snap back to hand for now
            // later you can add drop zone detection here
            transform.SetParent(_originalParent);
        }
    }
}
