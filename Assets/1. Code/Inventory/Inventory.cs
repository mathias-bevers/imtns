using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleanRoom.Inventory
{
    public class Inventory
    {
        private static InventoryItem[] _loadedItems;

        private int ItemCount => items.Sum(item => item.Value);
        private readonly Dictionary<InventoryItem, int> items = new();

        public Inventory(int initialItemCount)
        {
            LoadItems();
            AddForcedItems();
            AddRemainingItems(initialItemCount);
        }

        private void AddForcedItems()
        {
            for (int i = 0; i < _loadedItems.Length; ++i)
            {
                InventoryItem item = _loadedItems[i];
                if (!item.ForcedInInventory)
                {
                    continue;
                }

                items[item] = 1;
            }
        }

        private void AddRemainingItems(int initialItemCount)
        {
            while (ItemCount < initialItemCount)
            {
                InventoryItem item = _loadedItems[Random.Range(0, _loadedItems.Length)];
                Add(item);
            }
        }

        // after testing linq performance hit is negligible, faster in large data sets. 
        public InventoryItem[] GetItems() => items.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value)).ToArray();

        public bool Add(InventoryItem item)
        {
            if (!items.TryGetValue(item, out int itemCount))
            {
                items.Add(item, 1);
                return true;
            }

            if (itemCount == item.ItemLimit)
            {
                return false;
            }

            ++items[item];
            return true;
        }

        public bool Remove(InventoryItem item)
        {
            if (!items.TryGetValue(item, out int itemCount))
            {
                return false;
            }

            if (itemCount == 0)
            {
                return false;
            }

            --items[item];
            return true;
        }

        private static void LoadItems()
        {
            if (_loadedItems is { Length: > 0 })
            {
                return;
            }

            _loadedItems = Resources.LoadAll<InventoryItem>("InventoryItems");
        }


        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            sb.Append("INVENTORY OF ").Append(ItemCount).AppendLine(" ITEMS");

            foreach (KeyValuePair<InventoryItem, int> kvp in items)
            {
                sb.Append('\t').Append(kvp.Key.Name).Append(": ").AppendLine(kvp.Value.ToString("00"));
            }

            return sb.ToString();
        }
    }
}