using CleanRoom.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class LockerItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Transform parentAfterDrag { get; set; }
        
        private Canvas canvas;
        private Image image;
        private InventoryItem data;
        private Transform cachedTransform;
        

        public void Setup(InventoryItem data)
        {
            cachedTransform = transform;
            image = GetComponent<Image>();
            this.data = data;

            canvas ??= cachedTransform.parent.FindComponentUp<Canvas>();
            Debug.Log(canvas.name);

            image.sprite = this.data.Sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = cachedTransform.parent;
            cachedTransform.SetParent(canvas.transform, true);
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // throw new System.NotImplementedException();
            cachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // throw new System.NotImplementedException();
            image.raycastTarget = true;
            cachedTransform.SetParent(parentAfterDrag);
        }
    }
}