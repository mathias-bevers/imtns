using CleanRoom.Interactables;
using UnityEngine;

namespace CleanRoom
{
    public class Player : Singleton<Player>
    {
        [field: SerializeField] public InteractionButton interactionButton { get; private set; }
        [SerializeField] private int initialInventorySize = 3;

        public Inventory.Inventory inventory { get; private set; } //needs to be set in awake for resources-load.

        public override void Awake()
        {
            base.Awake();
            inventory = new Inventory.Inventory(initialInventorySize);
            Debug.Log(inventory.ToString());
        }
    }
}