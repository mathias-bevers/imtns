using System;
using System.Collections.Generic;
using System.Linq;
using CleanRoom.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CleanRoom.InventorySystem
{
    public class Inventory
    {
        private static InventoryItem[] _resources;
        
        private readonly List<InventoryItem> inventory;

        public Inventory(int initialItemCount)
        {
            inventory = new List<InventoryItem>();

            LoadItems();
            AddInitialItems(initialItemCount);
        }

        private void AddInitialItems(int initialItemCount)
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
            int index = inventory.IndexOf(item);
            
            if (index < 0)
            {
                return false;
            }

            inventory.RemoveAt(index);
            return true;
        }

        public InventoryItem[] GetInventory() => inventory.ToArray();

        public bool HasItem(string itemName) => inventory.Any(item => string.Equals(item.Name, itemName));
        
        private int GetItemCount(string itemName) => inventory.Count(item => string.Equals(item.Name, itemName));

        public static InventoryItem GetItemResource(string name)
        {
            return _resources.IsNullOrEmpty() ? null : Array.Find(_resources, item => item.Name == name);
        }

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