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

                if (items.TryAdd(item, 1))
                {
                    continue;
                }

                // if the item limit is equal to the items in the inventory, don't add another.
                if (item.ItemLimit == items[item])
                {
                    continue;
                }

                ++items[item];
            }
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