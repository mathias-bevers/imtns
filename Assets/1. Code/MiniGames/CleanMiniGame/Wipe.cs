using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Wipe : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private RectTransform cachedTransform;
        private Vector2 startingPosition;

        private void Awake()
        {
            cachedTransform = (RectTransform)transform;
            startingPosition = cachedTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            cachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cachedTransform.anchoredPosition = startingPosition;
        }
    }
}