using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.Movement
{
    public class OnScreenJoystick : MovementInput, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private RectTransform rectTransform;
        private RectTransform handleRectTransform;
        private Vector2 lastDirection;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            handleRectTransform = rectTransform.GetChild(0).GetComponent<RectTransform>();
            
            handleRectTransform.anchoredPosition = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position,
                eventData.pressEventCamera, out Vector2 position);

            Vector2 direction = position / (rectTransform.sizeDelta / 2);
            direction = Vector2.ClampMagnitude(direction, 1f);

            handleRectTransform.anchoredPosition = direction * (rectTransform.sizeDelta.x / 2);

            lastDirection = direction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            handleRectTransform.anchoredPosition = Vector2.zero;
            lastDirection = Vector2.zero;
        }

        public override Vector2 GetInput() => lastDirection;
    }
}