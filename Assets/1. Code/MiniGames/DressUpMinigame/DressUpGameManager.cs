using CleanRoom.MiniGames.CleanMiniGame;
using CleanRoom.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class DressUpGameManager : GameState
{
    [SerializeField] public List<ItemType> SlotOrder = new();
    [SerializeField] public List<ItemSlot> Slots = new();

    private GameObject GoggleItemSlot;
    private GameObject FacemaskItemSlot;

    private string NOT_CLOTHED_MESSAGE = "Je hebt niet alle kleding aangedaan";
    private string INCORRECT_ORDER_MESSAGE = "Kleding is in de verkeerde volgorde geplaatst: ";

    protected void OnEnable()
    {
        EnterEvent.AddListener(StartMiniGame);
        ExitEvent.AddListener(ValidateExit);
    }

    protected void OnDisable()
    {
        EnterEvent.RemoveAllListeners();
        ExitEvent.RemoveAllListeners();
    }

    private void StartMiniGame(){
        for (int i = 0; i < SlotOrder.Count; i++)
        {
            ItemSlot currentSlot = Slots[i];

            currentSlot.OnItemSlotted.AddListener(HandleItemSlotted);
            currentSlot.OnWrongItemPlaced.AddListener(ItemPlacedInWrongSlot);

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

        OnMistakeMade(NOT_CLOTHED_MESSAGE);
    }

    private void HandleItemSlotted(DragAndDropItem slottedItem)
    {
        if(slottedItem.ItemType == ItemType.Kap)
        {
            GoggleItemSlot.SetActive(true);
            FacemaskItemSlot.SetActive(true);
        }

        CheckItemOrder(slottedItem.ItemType);

        if (IsGameComplete())
        {
            Debug.Log("Dress-up minigame completed");
            Complete();
        }
    }

    private void CheckItemOrder(ItemType itemType)
    {
        if (IsItemInOrder(itemType)){
            SlotOrder.RemoveAt(0);
        }
        else
        {
            SlotOrder.Remove(itemType);
            OnMistakeMade(INCORRECT_ORDER_MESSAGE + itemType.ToString());
            Debug.Log(INCORRECT_ORDER_MESSAGE + itemType.ToString());
        }
    }

    private bool IsGameComplete()
    {
        return SlotOrder.Count == 0;
    }

    private bool IsItemInOrder(ItemType itemType)
    {
        return SlotOrder.ElementAt(0) == itemType;
    }

    private void ItemPlacedInWrongSlot(string message)
    {
        OnMistakeMade(message);
    }
}
