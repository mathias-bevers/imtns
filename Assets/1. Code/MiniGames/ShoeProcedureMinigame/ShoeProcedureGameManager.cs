using CleanRoom;
using CleanRoom.MiniGames.CleanMiniGame;
using CleanRoom.Menus;
using CleanRoom.MiniGames;
using KattenKasteel.FSM;
using UnityEngine;

public class ShoeProcedure : MiniGameManager<ShoeProcedure>
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

    [SerializeField] Vector3 NewPosition = Vector3.zero;
    
    protected void OnDisable()
    {
        ValidateExit();
    }

    protected override void StartMiniGame()
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

        GameManager.OnMistakeMade(StateName, NOT_ON_MESSAGE);
        Debug.Log(NOT_ON_MESSAGE);

        LeftShoeSlot.OnItemSlotted.RemoveAllListeners();
        RightShoeSlot.OnItemSlotted.RemoveAllListeners();
    }


    void OnRightShoeSlotted(DragAndDropItem slottedShoe)
    {
        if (RightShoe != slottedShoe.gameObject)
        {
            GameManager.OnMistakeMade(StateName, WRONG_FOOT_MESSAGE);
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
            GameManager.OnMistakeMade(StateName, WRONG_FOOT_MESSAGE);
            Debug.Log(WRONG_FOOT_MESSAGE);
            return;
        }

        if (!isRightShoeSlotted)
        {
            GameManager.OnMistakeMade(StateName, INCORRECT_ORDER_MESSAGE);
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
            CompleteMiniGame();
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