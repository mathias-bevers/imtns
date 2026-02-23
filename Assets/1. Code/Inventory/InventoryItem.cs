using UnityEngine;

namespace CleanRoom.Inventory
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "CleanRoom/InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        public enum Destination { Locker, CleanRoom, Trash }

        [field: SerializeField] public new string name { get; private set; }
        [field: SerializeField] public Sprite sprite { get; private set; }
        [field: SerializeField] public Destination destination { get; private set; }
        [field: SerializeField] public bool forcedInInventory { get; private set; }
        [field: SerializeField] public int itemLimit = 1;
        [field: SerializeField] public float scale { get; private set; } = 1f;
    }
}