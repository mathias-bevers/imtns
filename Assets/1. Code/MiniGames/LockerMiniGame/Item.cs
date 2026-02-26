using CleanRoom.Inventory;
using CleanRoom.StateMachine.GameStates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Transform CachedTransform { get; private set; }

        private static Canvas _canvas;
        private Image image;
        private InventoryItem data;


        public void Setup(InventoryItem data)
        {
            CachedTransform = transform;
            this.data = data;
            
            image = GetComponent<Image>();
            image.sprite = this.data.Sprite;

            if (!ReferenceEquals(null, _canvas))
            {
                return;
            }

            _canvas = CachedTransform.GetComponentInParents<LockerMiniGameState>().Canvas; 
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CachedTransform.SetParent(_canvas.transform, true);
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            CachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
        }
    }
}