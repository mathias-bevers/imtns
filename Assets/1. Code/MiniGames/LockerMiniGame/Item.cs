using CleanRoom.Inventory;
using CleanRoom.StateMachine.GameStates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public InventoryItem Data { get; private set; }
        public Transform TransformAfterDrag { get; set; }

        private static Canvas _canvas;
        private Transform cachedTransform = null;
        private Image image;


        public void Setup(InventoryItem data)
        {
            cachedTransform = transform;
            Data = data;

            image = GetComponent<Image>();
            image.sprite = Data.Sprite;
            name = Data.Name;

            if (!ReferenceEquals(null, _canvas))
            {
                return;
            }

            _canvas = cachedTransform.GetComponentInParents<LockerMiniGameState>().Canvas;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            TransformAfterDrag = cachedTransform.parent;
            cachedTransform.SetParent(_canvas.transform, true);
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            cachedTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            cachedTransform.SetParent(TransformAfterDrag);
        }
    }
}