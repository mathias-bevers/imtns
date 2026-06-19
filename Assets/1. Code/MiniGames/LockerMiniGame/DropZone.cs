using System;
using CleanRoom.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private InventoryItem.DestinationType destinationType;
        public event Action<InventoryItem, InventoryItem.DestinationType> itemDroppedEvent;


        public void OnDrop(PointerEventData eventData)
        {
            SortingItem sortingItem = eventData.pointerDrag.GetComponent<SortingItem>();

            if (ReferenceEquals(sortingItem, null))
            {
                return;
            }

            itemDroppedEvent?.Invoke(sortingItem.Data, destinationType);
        }
    }
}