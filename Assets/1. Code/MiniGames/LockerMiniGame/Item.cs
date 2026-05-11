using CleanRoom.InventorySystem;
using CleanRoom.NewStateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public InventoryItem Data { get; private set; }
        public Transform CachedTransform { get; private set;  }
        public Transform TransformAfterDrag { get; set; }

        private Canvas canvas;
        private Image image;

        public void Setup(InventoryItem data)
        {
            CachedTransform = transform;
            TransformAfterDrag = CachedTransform.parent;
            Data = data;

            image = GetComponent<Image>();
            image.sprite = Data.Sprite;
            name = Data.Name;

            if (!ReferenceEquals(null, canvas))
            {
                return;
            }

            canvas = CachedTransform.GetComponentInParents<SortingGameState>().Canvas;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            TransformAfterDrag = CachedTransform.parent;
            CachedTransform.SetParent(canvas.transform, true);
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            CachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            CachedTransform.SetParent(TransformAfterDrag);
        }
    }
}