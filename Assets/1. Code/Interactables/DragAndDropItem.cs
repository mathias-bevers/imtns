using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ItemType ItemType;
        [HideInInspector] public Vector3 startPosition;

        private CanvasGroup canvasGroup;
        private Image itemImage;

        private Vector3 offset;

        [field: SerializeField] public UnityEvent<ItemSlot> OnSlotted { get; private set; }
        [field: SerializeField] public UnityEvent<DragAndDropItem> OnPointerDownEvent { get; private set; }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            itemImage = GetComponent<Image>();
        }

        private void Start()
        {
            startPosition = transform.position;
        }

        public void OnPointerDown(PointerEventData eventData){
            OnPointerDownEvent.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;

            offset = (transform.position - (Vector3)eventData.position)/2;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = (Vector3)eventData.position + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = startPosition;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        //Snaps the item to the current position and disables it from being moved
        public void SnapAndDisable()
        {
            ExecuteEvents.endDragHandler.Invoke(this, new PointerEventData(EventSystem.current));
        }
    }
}