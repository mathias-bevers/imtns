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

    private const string NO_GLOVES = "Oeps! Je hebt geen handschoenen aan.";
    private const string ONE_GLOVE = "Oeps! Je hebt maar een handschoen aan.";

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
                OnMistakeMade(NO_GLOVES);
                break;
            case 1:
                OnMistakeMade(ONE_GLOVE);
                break;
        }
    }

    
    void HandleItemSlotted(ItemType itemType)
    {
        UsedGloves++;
        if (UsedGloves == Slots.Count)
        {
            Complete();
        }
    }
}
