using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.Movement
{
    public class OnScreenJoystick : MovementInput, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private const float MAX_DRAG = 45;

        private RectTransform rectTransform;
        private RectTransform handleRectTransform;
        private Vector2 direction;
        private float drag01;

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

            Vector2 newDirection = position / (rectTransform.sizeDelta / 2);
            newDirection = Vector2.ClampMagnitude(newDirection, 1f);

            handleRectTransform.anchoredPosition = newDirection * (rectTransform.sizeDelta.x / 2);

            drag01 = Vector2.Distance(handleRectTransform.position, rectTransform.position) / MAX_DRAG;
            drag01 = Mathf.Clamp01(drag01);

            direction = newDirection;
            direction.Normalize();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            handleRectTransform.anchoredPosition = Vector2.zero;
            direction = Vector2.zero;
        }

        public override Vector2 GetInput()
        {
            direction.Normalize();
            direction *= drag01;
            return direction;
        }
    }
}