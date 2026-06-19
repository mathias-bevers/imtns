using System.Collections.Generic;
using CleanRoom.MiniGames;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;

public class DressUpGameManager : MiniGameManager<DressUpGameManager>
{
    [SerializeField] private List<ItemSlot> Slots = new();
    [SerializeField] private GameObject hair;

    private GameObject GoggleItemSlot;
    private GameObject FacemaskItemSlot;

    private const string INCORRECT_ORDER_MESSAGE = "Kleding is in de verkeerde volgorde geplaatst: ";

    protected override void StartMiniGame()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            ItemSlot currentSlot = Slots[i];

            currentSlot.OnItemSlotted.AddListener(HandleItemSlotted);
            currentSlot.OnAttemptedToSlot.AddListener(ItemAttemptedToSlot);

            if (currentSlot.AllowedItems[0] == ItemType.Veiligheidsbril)
            {
                GoggleItemSlot = currentSlot.gameObject;
            }

            if (currentSlot.AllowedItems[0] == ItemType.Mondkapje)
            {
                FacemaskItemSlot = currentSlot.gameObject;
            }
        }

        if (GoggleItemSlot && FacemaskItemSlot)
        {
            GoggleItemSlot.SetActive(false);
            FacemaskItemSlot.SetActive(false);
        }
    }

    private void HandleItemSlotted(DragAndDropItem slottedItem)
    {
        switch (slottedItem.ItemType)
        {
            case ItemType.Kap:
                GoggleItemSlot.SetActive(true);
                FacemaskItemSlot.SetActive(true);
                hair.SetActive(false);

                EnableOverallSlot();
                break;
            case ItemType.Overall:
                EnableFacemaskSlot();
                break;
            case ItemType.Mondkapje:
                EnableGogglesSlot();
                break;
            case ItemType.Veiligheidsbril:
                CompleteMiniGame();
                break;
        }
    }

    private void ItemAttemptedToSlot(DragAndDropItem itemAttempted)
    {
        GameManager.OnMistakeMade(StateName, INCORRECT_ORDER_MESSAGE + itemAttempted.ItemType);
        Debug.Log(INCORRECT_ORDER_MESSAGE + itemAttempted.ItemType);
    }

    private void EnableOverallSlot()
    {
        foreach (ItemSlot slot in Slots)
        {
            if (slot.AllowedItems.Contains(ItemType.Overall))
            {
                slot.canSlot = true;
            }
        }
    }

    private void EnableGogglesSlot()
    {
        foreach (ItemSlot slot in Slots)
        {
            if (slot.AllowedItems.Contains(ItemType.Veiligheidsbril))
            {
                slot.canSlot = true;
            }
        }
    }

    private void EnableFacemaskSlot()
    {
        foreach (ItemSlot slot in Slots)
        {
            if (slot.AllowedItems.Contains(ItemType.Mondkapje))
            {
                slot.canSlot = true;
            }
        }
    }
}