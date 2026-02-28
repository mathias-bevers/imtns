using System.Globalization;
using CleanRoom.Inventory;
using CleanRoom.Menus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class ItemContainer : MonoBehaviour, IDropHandler
    {
        [field: SerializeField] public InventoryItem.DestinationType Destination { get; private set; }
        [field: SerializeField] public GridLayoutGroup Grid { get; private set; }
        [SerializeField] private RectTransform scrollRectTransform;

        private GridLayoutGroupResizer resizer;
        private int minHeight;

        private void OnEnable()
        {
            resizer = GetComponentInChildren<GridLayoutGroupResizer>();
            minHeight = Mathf.RoundToInt(scrollRectTransform.rect.height);
            resizer.Resize(minHeight);
        }


        public void OnDrop(PointerEventData eventData)
        {
            Item item = eventData.pointerDrag.GetComponent<Item>();

            if (ReferenceEquals(null, item))
            {
                return;
            }

            item.TransformAfterDrag = Grid.transform;
            resizer.Resize(minHeight);
            
            if (Destination == InventoryItem.DestinationType.CleanRoom)
            {
                Player.Instance.Inventory.Add(item.Data);
            }
            else
            {
                Player.Instance.Inventory.Remove(item.Data);
            }
        }
    }
}