using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleanRoom.InventorySystem
{
    public class Inventory
    {
        public int Size => inventory.Count;
        private readonly List<InventoryItem> inventory;

        public Inventory(InventoryContents initialContents)
        {
            inventory = new List<InventoryItem>(initialContents.Contents);
            int n = inventory.Count;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                (inventory[n], inventory[k]) = (inventory[k], inventory[n]);
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
        
        public bool HasItem(string itemName) => inventory.Any(item => string.Equals(item.Name, itemName));

        private int GetItemCount(string itemName) => inventory.Count(item => string.Equals(item.Name, itemName));

        public InventoryItem GetItemAtIndex(int i)
        {
            InventoryItem item = null;
            
            try
            {
                item = inventory[i];
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogError($"The index {i} is out of range");
            }

            return item;
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