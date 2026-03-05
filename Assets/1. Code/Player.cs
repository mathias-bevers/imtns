using CleanRoom.Interactables;
using UnityEngine;

namespace CleanRoom
{
    public class Player : Singleton<Player>
    {
        [SerializeField] private int initialInventorySize = 3;

        public Inventory.Inventory Inventory { get; private set; } //needs to be set in awake for resources-load.
        
        public override void Awake()
        {
            base.Awake();
            Inventory = new Inventory.Inventory(initialInventorySize);
        }
    }
}