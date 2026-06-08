using CleanRoom;
using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using System.Collections.Generic;
using KattenKasteel.FSM;
using UnityEngine;

public class ShoeRackGameManager : Singleton<ShoeRackGameManager>
{
    [SerializeField] public ItemSlot[] ShoeSlotPair1 = new ItemSlot[2];
    [SerializeField] public ItemSlot[] ShoeSlotPair2 = new ItemSlot[2];
    [SerializeField] public ItemSlot[] ShoeSlotPair3 = new ItemSlot[2];

    [SerializeField] private Transitioner onCompletionTransition;

    private List<ItemSlot[]> shoeSlotPairs = new();

    private string stateName = string.Empty;

    private string NOT_PAIRED_MESSAGE = "Schoenen zijn in de verkeerde plaats";

    [SerializeField] Vector3 NewPosition = Vector3.zero;

    protected void OnEnable()
    {
        stateName = StateMachine.Instance.ActiveState.StateName;
        StartMiniGame();
    }

    protected void OnDisable()
    {
        ValidateExit();
    }

    private void StartMiniGame()
    {
        shoeSlotPairs.Add(ShoeSlotPair1);
        shoeSlotPairs.Add(ShoeSlotPair2);
        shoeSlotPairs.Add(ShoeSlotPair3);

        foreach (ItemSlot[] curPair in shoeSlotPairs)
        {
            foreach (ItemSlot curShoeSlot in curPair)
            {
                curShoeSlot.OnItemSlotted.AddListener(OnShoePlaced);
            }
        }
    }

    private void ValidateExit()
    {
        if (IsGameComplete())
        {
            return;
        }

        GameManager.Instance.OnMistakeMade(stateName, NOT_PAIRED_MESSAGE);

        foreach (ItemSlot[] curPair in shoeSlotPairs)
        {
            foreach (ItemSlot curShoeSlot in curPair)
            {
                curShoeSlot.OnItemSlotted.RemoveAllListeners();
            }
        }
    }

    private void OnShoePlaced(DragAndDropItem currentItem)
    {
        ScaleShoe(currentItem);

        if (IsGameComplete())
        {
            StateMachine.Instance.CompleteActiveState();
            MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent += OnPopupClose;
        }
    }

    private void OnPopupClose(Popup.MessageType messageType)
    {
        if (messageType != Popup.MessageType.CompletedMiniGame)
        {
            return;
        }

        MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent -= OnPopupClose;
        onCompletionTransition.Transition();
    }

    private bool IsGameComplete()
    {
        bool wasPairMatched = false;

        //Check if any of the shoe slot pairs is filled with both shoes
        foreach (ItemSlot[] curPair in shoeSlotPairs)
        {
            bool pairMatched = true;

            foreach (ItemSlot curShoeSlot in curPair)
            {
                if (curShoeSlot.isEmpty)
                {
                    pairMatched = false;
                }
            }

            if (pairMatched)
            {
                wasPairMatched = true;
                break;
            }
        }

        return wasPairMatched;
    }

    private void ScaleShoe(DragAndDropItem currentItem)
    {
        Vector3 currentShoeScale = currentItem.gameObject.transform.localScale;

        //Check if the shoe image is flipped
        if (currentShoeScale.x > 0)
        {
            currentItem.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        }
        else
        {
            currentItem.gameObject.transform.localScale = new Vector3(-0.7f, 0.7f, 1);
        }
    }
}