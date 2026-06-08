using CleanRoom.MiniGames.CleanMiniGame;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Tooltip("List of items that are allowed in this slot. If list is empty, all items are allowed")]
    [SerializeField] public List<ItemType> AllowedItems;
    [SerializeField] protected bool hideOnSlot;

    public bool isEmpty = true;
    protected bool allowAllItems = false;
    protected DragAndDropItem slottedItem = null;

    private Image slotImage;
    private Color startingColor;
    private Color transparentColor;

    [field: SerializeField] public UnityEvent<DragAndDropItem> OnItemSlotted { get; private set; }
    [field: SerializeField] public UnityEvent<string> OnWrongItemPlaced { get; private set; }


    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    protected void Start()
    {
        startingColor = slotImage.color;
        transparentColor = new Color(255, 255, 255, 0);

        if (AllowedItems.Count == 0)
        {
            allowAllItems = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            TrySlotItem(eventData);
        }
    }

    protected void TrySlotItem(PointerEventData eventData)
    {
        if (!isEmpty) { return; }

        DragAndDropItem currentItem = eventData.pointerDrag.GetComponent<DragAndDropItem>();
        if (currentItem == null) { return; }

        if (IsItemAllowed(currentItem))
        {
            SlotItem(currentItem);

            if (hideOnSlot)
            {
                HideSlot();
            }
        }
        else
        {
            OnWrongItemPlaced.Invoke(currentItem.ItemType + " is verkeerd geplaatst");
        }
    }

    protected void SlotItem(DragAndDropItem currentItem)
    {
        currentItem.startPosition = transform.position;

        slottedItem = currentItem;
        isEmpty = false;

        slotImage.raycastTarget = false;

        currentItem.OnSlotted.Invoke(this);
        OnItemSlotted.Invoke(currentItem);

        currentItem.OnSlotted.AddListener(OnRemoveItem);
    }

    protected void OnRemoveItem(ItemSlot newItemSlot)
    {
        if(newItemSlot == this)
        {
            return;
        }

        slottedItem.OnSlotted.RemoveListener(OnRemoveItem);
        slottedItem = null;
        isEmpty = true;

        slotImage.raycastTarget = true;
        ShowSlot();

    }

    protected bool IsItemAllowed(DragAndDropItem item)
    {
        return allowAllItems || AllowedItems.Contains(item.ItemType);
    }

    public void ShowSlot()
    {
        slotImage.color = startingColor;
    }

    public void HideSlot()
    {
        slotImage.color = transparentColor;
    }
}
