using CleanRoom.MiniGames.CleanMiniGame;
using System.Collections.Generic;
using CleanRoom.MiniGames;
using UnityEngine;

public class ShoeRackGameManager : MiniGameManager<ShoeRackGameManager>
{
    [SerializeField] private ItemSlot[] ShoeSlotPair1 = new ItemSlot[2];
    [SerializeField] private ItemSlot[] ShoeSlotPair2 = new ItemSlot[2];
    [SerializeField] private ItemSlot[] ShoeSlotPair3 = new ItemSlot[2];
    
    private List<ItemSlot[]> shoeSlotPairs = new();

    protected override void StartMiniGame()
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

    private void OnShoePlaced(DragAndDropItem currentItem)
    {
        ScaleShoe(currentItem);

        if (IsGameComplete())
        {
            CompleteMiniGame();
        }
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