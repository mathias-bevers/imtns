using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class ItemContainer : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            LockerItem item = dropped.GetComponent<LockerItem>();
            item.parentAfterDrag = transform;
        }
    }
}