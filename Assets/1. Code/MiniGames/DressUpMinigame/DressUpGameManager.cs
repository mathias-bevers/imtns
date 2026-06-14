using CleanRoom.MiniGames.CleanMiniGame;
using System.Collections.Generic;
using System.Linq;
using CleanRoom;
using CleanRoom.Menus;
using CleanRoom.MiniGames;
using KattenKasteel.FSM;
using UnityEngine;

public class DressUpGameManager : MiniGameManager<DressUpGameManager>
{
    [SerializeField] public List<ItemSlot> Slots = new();
    [SerializeField] private GameObject hair;

    private GameObject GoggleItemSlot;
    private GameObject FacemaskItemSlot;


    private string NOT_CLOTHED_MESSAGE = "Je hebt niet alle kleding aangedaan";
    private string INCORRECT_ORDER_MESSAGE = "Kleding is in de verkeerde volgorde geplaatst: ";

    protected void OnDisable()
    {
        ValidateExit();
    }

    protected override void StartMiniGame(){
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

        if(GoggleItemSlot && FacemaskItemSlot)
        {
            GoggleItemSlot.SetActive(false);
            FacemaskItemSlot.SetActive(false);
        }
    }

    private void ValidateExit()
    {
        if (IsGameComplete())
        {
            return;
        }

        //GameManager.OnMistakeMade(StateName, NOT_CLOTHED_MESSAGE);
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
            default:
                break;
        }
    }

    private bool IsGameComplete()
    {
        bool areGlassesOn = false;

        foreach (ItemSlot slot in Slots)
        {
            if (slot.AllowedItems.Contains(ItemType.Veiligheidsbril))
            {
                areGlassesOn = !slot.isEmpty;
            }
        }

        return areGlassesOn;
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
