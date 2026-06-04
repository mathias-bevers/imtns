using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using CleanRoom.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GlovesGameState : GameState
{
    [SerializeField] public List<DragAndDropItem> Gloves = new();
    [SerializeField] public ItemSlot LeftGloveSlot;
    [SerializeField] public ItemSlot RightGloveSlot;
    [SerializeField] public GameObject GlovedHandLeft;
    [SerializeField] public GameObject GlovedHandRight;
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
        LeftGloveSlot.OnItemSlotted.AddListener(HandleLeftGloveSlotted);
        RightGloveSlot.OnItemSlotted.AddListener(HandleRightGloveSlotted);
        
        for (int i = 0; i < Gloves.Count; i++)
        {
            Gloves[i].OnPointerDownEvent.AddListener(ShowGlove);
        }
    }

    private void ValidateExit()
    {
        LeftGloveSlot.OnItemSlotted.RemoveAllListeners();
        RightGloveSlot.OnItemSlotted.RemoveAllListeners();

        for (int i = 0; i < Gloves.Count; i++)
        {
            Gloves[i].OnPointerDownEvent.RemoveAllListeners();
        }

        switch (UsedGloves)
        {
            case 0:
                OnMistakeMade(NO_GLOVES);
                break;
            case 1:
                OnMistakeMade(ONE_GLOVE);
                break;
        }
    }

    private void HandleLeftGloveSlotted(DragAndDropItem leftGlove)
    {
        leftGlove.gameObject.SetActive(false);
        GlovedHandLeft.SetActive(true);

        UsedGloves++;
        if (UsedGloves == 2)
        {
            Complete();
        }
    }

    private void HandleRightGloveSlotted(DragAndDropItem rightGlove)
    {
        rightGlove.gameObject.SetActive(false);
        GlovedHandRight.SetActive(true);

        UsedGloves++;
        if (UsedGloves == 2)
        {
            Complete();
        }
    }

    private void ShowGlove(DragAndDropItem currentGlove)
    {
        currentGlove.gameObject.GetComponent<Image>().color = Color.white;
    }
}
