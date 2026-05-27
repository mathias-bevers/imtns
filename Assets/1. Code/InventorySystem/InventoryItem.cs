using UnityEngine;

namespace CleanRoom.InventorySystem
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "CleanRoom/InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        public enum DestinationType { Locker, CleanRoom, Trash }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public DestinationType Destination { get; private set; }

        [field: SerializeField, TextArea] public string CorrectMessage { get; private set; }
        [field: SerializeField, TextArea] public string IncorrectMessage { get; private set; }
    }
}