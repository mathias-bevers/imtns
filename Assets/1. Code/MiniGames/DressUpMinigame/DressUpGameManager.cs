using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DressUpGameManager : MonoBehaviour
{
    [SerializeField] public List<ItemType> SlotOrder = new();
    [SerializeField] public List<ItemSlot> Slots = new();

    [field: SerializeField] public UnityEvent EnterEvent { get; private set; }
    [field: SerializeField] public UnityEvent ExitEvent { get; private set; }
    [field: SerializeField] public UnityEvent<string> OnMistake { get; private set; }

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
        for(int i = 0; i < SlotOrder.Count; i++)
        {
            Slots[i].OnItemSlotted.AddListener(HandleItemSlotted);
        }
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
    }

    void HandleItemSlotted(ItemType itemType)
    {
        CheckItemOrder(itemType);

        if (IsGameComplete())
        {
            Debug.Log("Dress-up minigame completed");
            ExitEvent.Invoke();

        }
    }

    void CheckItemOrder(ItemType itemType)
    {
        if (IsItemInOrder(itemType)){
            SlotOrder.RemoveAt(0);
        }
        else
        {
            SlotOrder.Remove(itemType);
            MistakeMade("Clothing placed in incorrect order: " + itemType.ToString());
            Debug.Log("Clothing placed in incorrect order: " + itemType.ToString());
        }
    }

    bool IsGameComplete()
    {
        return SlotOrder.Count == 0;
    }

    bool IsItemInOrder(ItemType itemType)
    {
        return SlotOrder.ElementAt(0) == itemType;
    }
}
