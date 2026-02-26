using System;
using CleanRoom.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Inventory
{
    public class InventoryUI : Menu
    {
        private const int CELL_HEIGHT = 325;
        private const int PADDING_Y = 25;

        [SerializeField] private RectTransform grid;
        [SerializeField] private GameObject inventoryItemPrefab;

        private void OnEnable()
        {
            openedEvent += LoadInventory;
        }

        private void LoadInventory()
        {
            InventoryItem[] inventoryItems = Player.Instance.Inventory.GetInventory();

            grid.DestroyAllChildren();

            int rowCount = Mathf.CeilToInt(inventoryItems.Length / 3f);
            grid.sizeDelta = new Vector2(grid.sizeDelta.x, rowCount * CELL_HEIGHT - PADDING_Y);

            for (int i = 0; i < inventoryItems.Length; ++i)
            {
                InventoryItem inventoryItem = inventoryItems[i];

                GameObject item = Instantiate(inventoryItemPrefab, grid);
                item.name = inventoryItem.Name;
                item.GetComponentInChildren<Image>().sprite = inventoryItem.Sprite;
                item.GetComponentInChildren<TextMeshProUGUI>().SetText(inventoryItem.Name);
            }
        }

        public void AddToInventory(InventoryItem item)
        {
            if (!Player.Instance.Inventory.Add(item))
            {
                throw new Exception($"Could not add the item {item.Name}");
            }

            LoadInventory();
        }

        public void RemoveFromInventory(InventoryItem item)
        {
            if (!Player.Instance.Inventory.Remove(item))
            {
                throw new Exception($"Could not remove the item {item.Name}");
            }

            LoadInventory();
        }
    }
}