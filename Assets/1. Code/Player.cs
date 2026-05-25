using CleanRoom.InventorySystem;
using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom
{
    public class Player : Singleton<Player>
    {
        [SerializeField, Expandable] private InventoryContents initialInventoryContents;

        public Inventory Inventory { get; private set; } //needs to be set in awake for resources-load.

        public override void Awake()
        {
            base.Awake();
            Inventory = new Inventory(initialInventoryContents);
        }
    }
}