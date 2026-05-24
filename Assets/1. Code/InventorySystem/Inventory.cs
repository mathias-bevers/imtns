using System;
using System.Collections.Generic;
using System.Linq;
using CleanRoom.Utils;
using UnityEngine;

namespace CleanRoom.InventorySystem
{
    public class Inventory
    {
        private static InventoryItem[] _resources;

        private readonly List<InventoryItem> inventory;

        public Inventory(InventoryContents initialContents)
        {
            inventory = new List<InventoryItem>();

            LoadItems();
         
            for (int i = 0; i < initialContents.Contents.Length; ++i)
            {
                inventory.Add(initialContents.Contents[i]);
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

        public static InventoryItem GetItemResource(string name) =>
            _resources.IsNullOrEmpty() ? null : Array.Find(_resources, item => item.Name == name);

        private static void LoadItems()
        {
            if (_resources.IsNullOrEmpty())
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