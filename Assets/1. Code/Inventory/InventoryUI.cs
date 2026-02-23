using CleanRoom.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Inventory
{
    public class InventoryUI : Menu
    {
        [SerializeField] private Transform grid;
        [SerializeField] private GameObject inventoryItemPrefab;
        
        

        private void OnEnable()
        {
            openedEvent += LoadInventory;
        }

        private void LoadInventory()
        {
            InventoryItem[] inventoryItems = Player.Instance.Inventory.GetItems();

            for (int i = grid.childCount - 1; i >= 0; --i)
            {
                Destroy(grid.GetChild(i).gameObject);
            }

            for (int i = 0; i < inventoryItems.Length; ++i)
            {
                InventoryItem inventoryItem = inventoryItems[i];

                GameObject item = Instantiate(inventoryItemPrefab, grid);
                item.name = inventoryItem.Name;
                item.GetComponentInChildren<Image>().sprite = inventoryItem.Sprite;
                item.GetComponentInChildren<TextMeshProUGUI>().SetText(inventoryItem.Name);
            }
        }
    }
}