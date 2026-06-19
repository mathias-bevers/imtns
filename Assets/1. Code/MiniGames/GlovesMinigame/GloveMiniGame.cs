using System.Collections.Generic;
using CleanRoom.MiniGames;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;
using UnityEngine.UI;

public class GloveMiniGame : MiniGameManager<GloveMiniGame>
{
    [SerializeField] private List<DragAndDropItem> Gloves = new();
    [SerializeField] private ItemSlot LeftGloveSlot;
    [SerializeField] private ItemSlot RightGloveSlot;
    [SerializeField] private GameObject GlovedHandLeft;
    [SerializeField] private GameObject GlovedHandRight;

    private int UsedGloves = 0;

    protected override void StartMiniGame()
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
            CompleteMiniGame();
        }
    }

    private void HandleRightGloveSlotted(DragAndDropItem rightGlove)
    {
        rightGlove.gameObject.SetActive(false);
        GlovedHandRight.SetActive(true);

        UsedGloves++;
        if (UsedGloves == 2)
        {
            CompleteMiniGame();
        }
    }

    private void ShowGlove(DragAndDropItem currentGlove)
    {
        currentGlove.gameObject.GetComponent<Image>().color = Color.white;
    }
}