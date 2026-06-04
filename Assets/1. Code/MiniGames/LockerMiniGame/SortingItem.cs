using CleanRoom.InventorySystem;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class SortingItem : DragAndSnap
    {
        private Image image;
        public Image Image
        {
            get
            {
                image ??= GetComponent<Image>();
                return image;
            }
        }
        public InventoryItem Data { get; private set; }

        public void Setup(InventoryItem data)
        {
            Data = data;
            Image.sprite = Data.Sprite;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            Image.raycastTarget = false;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            Image.raycastTarget = true;
        }
    }
}