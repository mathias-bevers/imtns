using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DragAndSnap : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private RectTransform cachedTransform = null;
        public RectTransform CachedTransform
        {
            get
            {
                if (ReferenceEquals(null, cachedTransform))
                {
                    cachedTransform = (RectTransform)transform;
                }

                return cachedTransform;
            }
        }

        private bool draggable = true;
        private Vector2 startingPosition;

        private void Awake()
        {
            startingPosition = CachedTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!draggable)
            {
                return;
            }

            CachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CachedTransform.anchoredPosition = startingPosition;
        }

        public void SnapAndDisable()
        {
            ExecuteEvents.endDragHandler.Invoke(this, new PointerEventData(EventSystem.current));
            draggable = false;
        }
    }
}