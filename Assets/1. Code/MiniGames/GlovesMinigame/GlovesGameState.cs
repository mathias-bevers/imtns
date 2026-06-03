using CleanRoom.Menus;
using CleanRoom.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GlovesGameState : GameState
{
    [SerializeField] public List<ItemSlot> Slots = new();
    [SerializeField] private int UsedGloves = 0;

    private const string NO_GLOVES = "Oeps, de tablet was nog niet helemaal school!";
    private const string ONE_GLOVE = "Oeps, de tablet was nog niet helemaal school!";

    protected void OnEnable()
    {
        EnterEvent.AddListener(StartMiniGame);
        ExitEvent.AddListener(ValidateExit);
    }

    protected void OnDisable()
    {
        ExitEvent.RemoveListener(ValidateExit);
        EnterEvent.RemoveListener(StartMiniGame);
    }

    private void StartMiniGame()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].OnItemSlotted.AddListener(HandleItemSlotted);
        }
    }

    private void ValidateExit()
    {
        switch(UsedGloves)
        {
            case 0:
                MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(NO_GLOVES, Popup.MessageType.Incorrect);
                break;
            case 1:
                MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(ONE_GLOVE, Popup.MessageType.Incorrect);
                break;
            case 2:
                Complete();
                break;

        }
    }


    void HandleItemSlotted(ItemType itemType)
    {
        UsedGloves++;
    }

    bool IsGameComplete()
    {
        return UsedGloves == 2;
    }
}
