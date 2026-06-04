using CleanRoom.MiniGames.CleanMiniGame;
using System.Collections.Generic;
using System.Linq;
using CleanRoom;
using CleanRoom.Menus;
using KattenKasteel.FSM;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ShoeProcedure : Singleton<ShoeProcedure>
{
    [SerializeField] private GameObject LeftShoe;
    [SerializeField] private GameObject RightShoe;
    [SerializeField] private ItemSlot LeftShoeSlot;
    [SerializeField] private ItemSlot RightShoeSlot;
    [SerializeField] private Animator animator;
    [SerializeField] private Transitioner onCompletionTransition;

    private bool isRightShoeSlotted = false;
    private bool isLeftShoeSlotted = false;

    private string stateName = string.Empty;

    private string INCORRECT_ORDER_MESSAGE = "Je hebt de schoen in de verkeerde volgorde geplaatst";
    private string WRONG_FOOT_MESSAGE = "Je hebt de schoen op de verkeerde foot geplaatst";
    private string NOT_ON_MESSAGE = "Schoenen zijn niet aangedaan";

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
        LeftShoeSlot.OnItemSlotted.AddListener(OnLeftShoeSlotted);
        RightShoeSlot.OnItemSlotted.AddListener(OnRightShoeSlotted);
    }

    private void ValidateExit()
    {
        if (IsGameComplete())
        {
            return;
        }

        GameManager.Instance.OnMistakeMade(stateName, NOT_ON_MESSAGE);
        Debug.Log(NOT_ON_MESSAGE);

        LeftShoeSlot.OnItemSlotted.RemoveAllListeners();
        RightShoeSlot.OnItemSlotted.RemoveAllListeners();
    }


    void OnRightShoeSlotted(DragAndDropItem slottedShoe)
    {
        if (RightShoe != slottedShoe.gameObject)
        {
            GameManager.Instance.OnMistakeMade(stateName, WRONG_FOOT_MESSAGE);
            Debug.Log(WRONG_FOOT_MESSAGE);
            return;
        }

        isRightShoeSlotted = true;

        PlayOnRightShoeAnimation();
    }

    void OnLeftShoeSlotted(DragAndDropItem slottedShoe)
    {
        if (LeftShoe != slottedShoe.gameObject)
        {
            GameManager.Instance.OnMistakeMade(stateName, WRONG_FOOT_MESSAGE);
            Debug.Log(WRONG_FOOT_MESSAGE);
            return;
        }

        if (!isRightShoeSlotted)
        {
            GameManager.Instance.OnMistakeMade(stateName, INCORRECT_ORDER_MESSAGE);
            Debug.Log(INCORRECT_ORDER_MESSAGE);
            return;
        }

        isLeftShoeSlotted = true;
        PlayOnRightShoeAnimation();

        CheckGameCompletion();
    }

    void CheckGameCompletion()
    {
        if (IsGameComplete())
        {
            Debug.Log("Shoe procedure completed");
            StateMachine.Instance.CompleteActiveState();
            MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent += OnCompletePopupClose;
        }
    }

    private void OnCompletePopupClose(Popup.MessageType messageType)
    {
        if (messageType != Popup.MessageType.CompletedMiniGame)
        {
            return;
        }


        MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent -= OnCompletePopupClose;
        onCompletionTransition.Transition();
    }

    bool IsGameComplete()
    {
        return isRightShoeSlotted && isLeftShoeSlotted;
    }

    void PlayOnRightShoeAnimation()
    {
        //Play the animation of the right foot moving to the other side

        //animator.Play();
    }


    void PlayOnLeftShoeAnimation()
    {
        //Play the animation of the left foot moving to the other side

        //animator.Play();
    }
}