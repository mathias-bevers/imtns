using CleanRoom;
using CleanRoom.MiniGames.CleanMiniGame;
using CleanRoom.MiniGames;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ShoeProcedure : MiniGameManager<ShoeProcedure>
{
    [SerializeField] private List<DragAndDropItem> socks = new();
    [SerializeField] private List<DragAndDropItem> slippers = new();
    [SerializeField] private ItemSlot footSlot;
    [SerializeField] private ItemSlot footSlot2;

    [Header("Buttons")]
    [SerializeField] private Button benchButton;
    [SerializeField] private Button groundButton;

    [Header("Frames")]
    [SerializeField] private Image background;
    [SerializeField] private List<Sprite> framesList = new();

    private int currentFrame = 0;

    private bool isSock1Slotted = false;
    private bool isSlipper1Slotted = false;
    private bool isFirstFootOnGround = false;
    private bool isSecondFootOnBench = false;
    private bool isSock2Slotted = false;
    private bool isSlipper2Slotted = false;
    private bool areBothFeetOnTheGround = false;

    private string INCORRECT_ORDER_MESSAGE = "Je hebt de sok of schoen in de verkeerde volgorde geplaatst";
    private string NOT_COMPLETED_MESSAGE = "Schoenen zijn niet aangedaan";
    
    protected void OnDisable()
    {
        ValidateExit();
    }

    protected override void StartMiniGame()
    {
        footSlot.OnItemSlotted.AddListener(OnFootItemSlotted);
        footSlot.OnWrongItemSlotted.AddListener(OnWrongItemSlotted);

        footSlot2.OnItemSlotted.AddListener(OnFootItemSlotted);
        footSlot2.OnWrongItemSlotted.AddListener(OnWrongItemSlotted);

        foreach (DragAndDropItem sock in socks)
        {
            sock.gameObject.SetActive(false);
        }

        foreach (DragAndDropItem slipper in slippers)
        {
            slipper.gameObject.SetActive(false);
        }

        footSlot.gameObject.SetActive(false);
        footSlot2.gameObject.SetActive(false);
        groundButton.gameObject.SetActive(false);
    }

    private void ValidateExit()
    {
        if (IsGameComplete())
        {
            return;
        }

        GameManager.OnMistakeMade(StateName, NOT_COMPLETED_MESSAGE);
        Debug.Log(NOT_COMPLETED_MESSAGE);

        footSlot.OnItemSlotted.RemoveAllListeners();
    }


    protected void AdvanceFrames()
    {
        currentFrame++;
        currentFrame = currentFrame.Clamp(0, framesList.Count - 1);

        background.sprite = framesList[currentFrame];
    }

    public void OnButtonPress()
    {
        //Disable buttons
        benchButton.gameObject.SetActive(false);
        groundButton.gameObject.SetActive(false);

        if (!isSock1Slotted)
        {
            //Enable sock 1 and slipper 1
            socks[0].gameObject.SetActive(true);
            slippers[0].gameObject.SetActive(true);

            DisplayFootSlot();
            AdvanceFrames();
            return;
        }

        if (!isSlipper1Slotted){ return; }

        if (!isFirstFootOnGround) {
            isFirstFootOnGround = true;

            benchButton.gameObject.SetActive(true);

            AdvanceFrames();
            return; 
        }

        if (!isSecondFootOnBench)
        {
            isSecondFootOnBench = true;

            //Enable sock 2 and slipper 2
            socks[1].gameObject.SetActive(true);
            slippers[1].gameObject.SetActive(true);

            DisplayFootSlot2();

            AdvanceFrames();
            return;
        }

        if (!areBothFeetOnTheGround)
        {
            areBothFeetOnTheGround = true;

            groundButton.gameObject.SetActive(true);

            AdvanceFrames();
            return;
        }

        if (areBothFeetOnTheGround)
        {
            AdvanceFrames();
            CheckGameCompletion();
        }
    }

    void OnFootItemSlotted(DragAndDropItem slottedShoe)
    {
        if (!isSock1Slotted){
            isSock1Slotted = true;

            //Hide the first sock
            socks[0].gameObject.SetActive(false);

            //Allow the shoe to be placed
            footSlot.SetAllowedItem(ItemType.Shoen);

            DisplayFootSlot();

            AdvanceFrames();
            return;
        }

        if (!isSlipper1Slotted)
        {
            isSlipper1Slotted = true;

            //Hide the first slipper
            slippers[0].gameObject.SetActive(false);

            //Disable the foot slot
            footSlot.gameObject.SetActive(false);

            //Disable the bench button
            benchButton.gameObject.SetActive(false);

            //Enable the ground button
            groundButton.gameObject.SetActive(true);

            //Allow the sock to be placed for the next foot
            footSlot.SetAllowedItem(ItemType.Sok);

            AdvanceFrames();
            return;
        }

        if (!isSock2Slotted)
        {
            isSock2Slotted = true;

            //Hide the first sock
            socks[1].gameObject.SetActive(false);

            //Allow the shoe to be placed
            footSlot2.SetAllowedItem(ItemType.Shoen);

            DisplayFootSlot2();

            AdvanceFrames();
            return;
        }

        if (!isSlipper2Slotted)
        {
            isSlipper2Slotted = true;

            //Hide the first slipper
            slippers[1].gameObject.SetActive(false);

            //Disable the foot slot
            footSlot.gameObject.SetActive(false);

            //Disable the bench button
            benchButton.gameObject.SetActive(false);

            //Enable the ground button
            groundButton.gameObject.SetActive(true);

            AdvanceFrames();
            return;
        }
    }


    void OnWrongItemSlotted(string wrongItemMessage)
    {
        GameManager.OnMistakeMade(StateName, INCORRECT_ORDER_MESSAGE);
        Debug.Log(INCORRECT_ORDER_MESSAGE);
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
        return currentFrame >= framesList.Count - 1;
    }

    void DisplayFootSlot()
    {
        footSlot.EmptySlot();
        footSlot.gameObject.SetActive(true);
    }

    void DisplayFootSlot2()
    {
        footSlot2.EmptySlot();
        footSlot2.gameObject.SetActive(true);
    }
}