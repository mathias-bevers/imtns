using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DragAndSnap : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public bool Draggable { get; private set; } = true;

        private RectTransform cachedTransform;
        private Vector2 startingPosition;

        private void Awake()
        {
            cachedTransform = (RectTransform)transform;
            startingPosition = cachedTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Draggable)
            {
                return;
            }

            cachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cachedTransform.anchoredPosition = startingPosition;
        }

        public void SnapAndDisable()
        {
            cachedTransform.anchoredPosition = startingPosition;
            Draggable = false;
        }
    }
}