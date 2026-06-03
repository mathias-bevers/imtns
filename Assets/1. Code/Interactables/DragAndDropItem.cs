using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Collections.Generic;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ItemType ItemType;
        [HideInInspector] public Vector3 startPosition;

        protected Vector3 offset;

        private CanvasGroup canvasGroup;
        private Image itemImage;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            itemImage = GetComponent<Image>();
        }

        private void Start()
        {
            startPosition = transform.position;
        }

        public void OnPointerDown(PointerEventData eventData){   }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;

            offset = transform.position - (Vector3)eventData.position;
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