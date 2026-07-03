using System.Globalization;
using CleanRoom.InventorySystem;
using CleanRoom.MiniGames.CleanMiniGame;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class SortingItem : DragAndSnap
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        public InventoryItem Data { get; private set; }

        public void SetActive(bool isActive)
        {
            image.gameObject.SetActive(isActive);
            text.gameObject.SetActive(isActive);
        }


        public void DisplayData(InventoryItem data)
        {
            Data = data;
            image.sprite = Data.Sprite;
            string capitalizedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name);
            text.SetText(capitalizedName);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            image.raycastTarget = true;
        }
    }
}