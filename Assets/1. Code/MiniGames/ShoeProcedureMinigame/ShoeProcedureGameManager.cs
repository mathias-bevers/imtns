using CleanRoom.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ShoeProcedureGameManager : GameState
{
    [SerializeField] private ItemSlot RightShoeSlot;
    [SerializeField] private ItemSlot LeftShoeSlot;
    [SerializeField] private Animator animator;

    [field: SerializeField] public UnityEvent<string> OnMistake { get; private set; }


    private bool isRightShoeSlotted = false;
    private bool isLeftShoeSlotted = false;
    private bool isAnimationComplete = true;

    protected void OnEnable()
    {
        StartMiniGame();
    }

    protected void OnDisable()
    {
        ValidateExit();
    }

    private void Start()
    {
        RightShoeSlot.OnItemSlotted.AddListener(OnRightShoeSlotted);
        LeftShoeSlot.OnItemSlotted.AddListener(OnLeftShoeSlotted);
        
    }

    private void StartMiniGame()
    {
        EnterEvent.Invoke();
    }

    private void ValidateExit()
    {
        ExitEvent.Invoke();
    }

    private void MistakeMade(string mistakeDescription)
    {
        OnMistake.Invoke(mistakeDescription);
        Debug.Log(mistakeDescription);
    }


    void OnRightShoeSlotted(ItemType itemType)
    {
        isRightShoeSlotted = true;

        PlayOnRightShoeAnimation();
    }

    void OnLeftShoeSlotted(ItemType itemType)
    {
        if (!isRightShoeSlotted)
        {
            MistakeMade("You put shoes in the wrong order");
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
            ExitEvent.Invoke();
        }
    }

    bool IsGameComplete()
    {
        return isRightShoeSlotted && isLeftShoeSlotted && isAnimationComplete;
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
