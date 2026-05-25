using CleanRoom.InventorySystem;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class SortingItem : DragAndSnap
    {
        public Image Image { get; private set; }
        public InventoryItem Data { get; private set; }

        private void Awake()
        {
            Image = GetComponent<Image>();
        }

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