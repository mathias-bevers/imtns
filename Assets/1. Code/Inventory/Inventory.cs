using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleanRoom.Inventory
{
    public class Inventory
    {
        private static InventoryItem[] _resources;

        private readonly List<InventoryItem> inventory;

        public Inventory(int initialItemCount)
        {
            inventory = new List<InventoryItem>();
            
            LoadItems();
            AddForcedItems();
            AddRemainingItems(initialItemCount);
        }

        private void AddForcedItems()
        {
            for (int i = 0; i < _resources.Length; ++i)
            {
                InventoryItem item = _resources[i];
                if (!item.ForcedInInventory)
                {
                    continue;
                }

                inventory.Add(item);
            }
        }

        private void AddRemainingItems(int initialItemCount)
        {
            while (inventory.Count < initialItemCount)
            {
                InventoryItem item = _resources[Random.Range(0, _resources.Length)];

                if (item.SkipInRandomization)
                {
                    continue;
                }

                Add(item);
            }
        }

        public bool Add(InventoryItem item)
        {
            if (GetItemCount(item.Name) >= item.ItemLimit)
            {
                return false;
            }

            inventory.Add(item);
            return true;
        }

        public bool Remove(InventoryItem item)
        {
            int itemCount = GetItemCount(item.Name);
            if (itemCount <= 0)
            {
                return false;
            }

            inventory.Remove(item);
            return true;
        }

        public InventoryItem[] GetInventory() => inventory.ToArray();

        private int GetItemCount(string itemName) =>
            inventory.Count(inventoryItem => string.Equals(inventoryItem.Name, itemName));

        private static void LoadItems()
        {
            if (_resources is { Length: > 0 })
            {
                return;
            }

            _resources = Resources.LoadAll<InventoryItem>("InventoryItems");
        }


        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            sb.Append("INVENTORY OF ").Append(inventory.Count).AppendLine(" ITEMS");
            foreach (string name in inventory.Select(item => item.Name).Distinct())
            {
                sb.Append('\t').Append(name).AppendLine(GetItemCount(name).ToString("00"));
            }

            return sb.ToString();
        }
    }
}