using CleanRoom.MiniGames.CleanMiniGame;
using System.Collections.Generic;
using CleanRoom;
using KattenKasteel.FSM;
using UnityEngine;
using UnityEngine.UI;

public class GlovesGameState : Singleton<GlovesGameState>
{
    [SerializeField] public List<DragAndDropItem> Gloves = new();
    [SerializeField] public ItemSlot LeftGloveSlot;
    [SerializeField] public ItemSlot RightGloveSlot;
    [SerializeField] public GameObject GlovedHandLeft;
    [SerializeField] public GameObject GlovedHandRight;
    [SerializeField] private int UsedGloves = 0;

    protected void OnEnable()
    {
        StartMiniGame();
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

    private void HandleLeftGloveSlotted(DragAndDropItem leftGlove)
    {
        leftGlove.gameObject.SetActive(false);
        GlovedHandLeft.SetActive(true);

        UsedGloves++;
        if (UsedGloves == 2)
        {
            StateMachine.Instance.CompleteActiveState();
        }
    }

    private void HandleRightGloveSlotted(DragAndDropItem rightGlove)
    {
        rightGlove.gameObject.SetActive(false);
        GlovedHandRight.SetActive(true);

        UsedGloves++;
        if (UsedGloves == 2)
        {
            StateMachine.Instance.CompleteActiveState();
        }
    }

    private void ShowGlove(DragAndDropItem currentGlove)
    {
        currentGlove.gameObject.GetComponent<Image>().color = Color.white;
    }
}
