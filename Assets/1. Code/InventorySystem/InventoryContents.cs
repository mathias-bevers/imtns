using UnityEngine;

namespace CleanRoom.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryContents", menuName = "CleanRoom/InventoryContents", order = 0)]
    public class InventoryContents : ScriptableObject
    {
        [field: SerializeField] public InventoryItem[] Contents { get; private set; }
    }
}