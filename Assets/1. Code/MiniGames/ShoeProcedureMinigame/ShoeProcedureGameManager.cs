using CleanRoom.MiniGames.CleanMiniGame;
using CleanRoom.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ShoeProcedure : GameState
{
    [SerializeField] private GameObject LeftShoe;
    [SerializeField] private GameObject RightShoe;
    [SerializeField] private ItemSlot LeftShoeSlot;
    [SerializeField] private ItemSlot RightShoeSlot;
    [SerializeField] private Animator animator;

    private bool isRightShoeSlotted = false;
    private bool isLeftShoeSlotted = false;

    private string INCORRECT_ORDER_MESSAGE = "Je hebt de schoen in de verkeerde volgorde geplaatst";
    private string WRONG_FOOT_MESSAGE = "Je hebt de schoen op de verkeerde foot geplaatst";
    private string NOT_ON_MESSAGE = "Schoenen zijn niet aangedaan";

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
        LeftShoeSlot.OnItemSlotted.AddListener(OnLeftShoeSlotted);
        RightShoeSlot.OnItemSlotted.AddListener(OnRightShoeSlotted);
    }

    private void ValidateExit()
    {
        if (IsGameComplete())
        {
            return;
        }

        OnMistakeMade(NOT_ON_MESSAGE);
        Debug.Log(NOT_ON_MESSAGE);

        LeftShoeSlot.OnItemSlotted.RemoveAllListeners();
        RightShoeSlot.OnItemSlotted.RemoveAllListeners();
    }


    void OnRightShoeSlotted(DragAndDropItem slottedShoe)
    {
        if(RightShoe != slottedShoe.gameObject)
        {
            OnMistakeMade(WRONG_FOOT_MESSAGE);
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
            OnMistakeMade(WRONG_FOOT_MESSAGE);
            Debug.Log(WRONG_FOOT_MESSAGE);
            return;
        }

        if (!isRightShoeSlotted)
        {
            OnMistakeMade(INCORRECT_ORDER_MESSAGE);
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
            Complete();
        }
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
