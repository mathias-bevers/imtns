using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DragAndSnap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform cachedTransform = null;
        public RectTransform CachedTransform
        {
            get
            {
                cachedTransform ??= (RectTransform)transform;
                return cachedTransform;
            }
        }

        private bool draggable = true;
        private Vector2 startingPosition;

        private void Awake()
        {
            startingPosition = CachedTransform.anchoredPosition;
        }
        
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!draggable)
            {
                eventData.pointerDrag = null;
                return;
            }

            CachedTransform.position = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            CachedTransform.anchoredPosition = startingPosition;
        }

        public void SnapAndDisable()
        {
            ExecuteEvents.endDragHandler.Invoke(this, new PointerEventData(EventSystem.current));
            draggable = false;
        }

        public void SnapAndDisableTemporarily()
        {
            ExecuteEvents.endDragHandler.Invoke(this, new PointerEventData(EventSystem.current));
            StartCoroutine(DisableAndEnableAfterDelay());
        }

        private IEnumerator DisableAndEnableAfterDelay()
        {
            draggable = false;
            yield return new WaitForSecondsRealtime(0.1f);
            draggable = true;
        }
    }
}