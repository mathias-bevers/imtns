using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using CleanRoom.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Tilemaps.TilemapRenderer;

public class ShoeRackGameManager : GameState
{
    [SerializeField] public ItemSlot[] ShoeSlotPair1 = new ItemSlot[2];
    [SerializeField] public ItemSlot[] ShoeSlotPair2 = new ItemSlot[2];
    [SerializeField] public ItemSlot[] ShoeSlotPair3 = new ItemSlot[2];

    private List<ItemSlot[]> shoeSlotPairs = new();

    private string NOT_PAIRED_MESSAGE = "Schoenen zijn in de verkeerde plaats";

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

    private void Start()
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

    private void StartMiniGame(){}

    private void ValidateExit()
    {
        if (IsGameComplete())
        {
            return;
        }

        OnMistakeMade(NOT_PAIRED_MESSAGE);
    }

    private void OnShoePlaced(DragAndDropItem currentItem)
    {
        ScaleShoe(currentItem);

        if (IsGameComplete())
        {
            Debug.Log("Shoe rack minigame completed");
            Complete();
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

            if (pairMatched) {
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
