using CleanRoom.Menus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class ItemContainer : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Inventory.InventoryItem.DestinationType destination;
        private GridLayoutGroupResizer resizer;
        private float minHeight;

        private void OnEnable()
        {
            resizer = GetComponent<GridLayoutGroupResizer>();
            minHeight = ((RectTransform)transform.parent).rect.height;
            resizer.Resize(minHeight);
        }


        public void OnDrop(PointerEventData eventData)
        {
            Item item = eventData.pointerDrag.GetComponent<Item>();

            if (ReferenceEquals(null, item))
            {
                return;
            }

            item.CachedTransform.SetParent(transform);

            resizer.Resize(minHeight);
        }
    }
}