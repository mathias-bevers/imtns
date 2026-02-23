using UnityEngine;

namespace CleanRoom.Inventory
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "CleanRoom/InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        public enum DestinationType { Locker, CleanRoom, Trash }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public DestinationType Destination { get; private set; }
        [field: SerializeField] public bool ForcedInInventory { get; private set; }
        [field: SerializeField] public int ItemLimit { get; private set; } = 1;
        [field: SerializeField] public float Scale { get; private set; } = 1f;
    }
}