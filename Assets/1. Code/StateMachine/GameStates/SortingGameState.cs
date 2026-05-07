using System;
using System.Collections.Generic;
using CleanRoom.Inventory;
using CleanRoom.Menus;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.NewStateMachine
{
    public class SortingGameState : GameState
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }

        [SerializeField] private Item itemPrefab;
        private Dictionary<InventoryItem.DestinationType, ItemContainer> containers;
        private PopupMenu popupMenu;
        private Transform cleanRoomContainer;

        private void OnEnable()
        {
            EnterEvent.AddListener(StartMiniGame);
            ExitEvent.AddListener(ValidateItems);
        }

        private void OnDisable()
        {
            EnterEvent.RemoveListener(StartMiniGame);
            ExitEvent.RemoveListener(ValidateItems);
        }

        private void StartMiniGame()
        {
            popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>();
            containers = new Dictionary<InventoryItem.DestinationType, ItemContainer>();

            foreach (ItemContainer container in GetComponentsInChildren<ItemContainer>(true))
            {
                containers.Add(container.Destination, container);
                container.itemDroppedEvent += OnItemDropped;
            }

            LoadItems();
        }

        private void OnItemDropped(bool isCorrect)
        {
            if (!isCorrect)
            {
                popupMenu.CreatePopup("dat was niet correct!", Popup.Level.Warning);
            }

            ValidateItems(false);
        }

        private void PopulateContainer(Transform container, InventoryItem[] items)
        {
            if (ReferenceEquals(container, null))
            {
                throw new NullReferenceException("container is null");
            }

            container.DestroyAllChildren();

            if (items.IsNullOrEmpty())
            {
                return;
            }

            for (int i = 0; i < items.Length; ++i)
            {
                Item item = Instantiate(itemPrefab, container);
                item.Setup(items[i]);
            }
        }

        private void LoadItems()
        {
            PopulateContainer(containers[InventoryItem.DestinationType.CleanRoom].Grid.transform,
                Player.Instance.Inventory.GetInventory());

            //TODO: load for locker and trash
        }

        private void ValidateItems() => ValidateItems(true);

        private void ValidateItems(bool isClosing)
        {
            int mistakes = 0;
            string log = string.Empty;

            foreach (ItemContainer container in containers.Values)
            {
                Item[] contents = container.Grid.GetComponentsInChildren<Item>();
                for (int i = 0; i < contents.Length; ++i)
                {
                    if (container.Destination == contents[i].Data.Destination)
                    {
                        continue;
                    }

                    log += $"{contents[i].name}: {container.Destination} != {contents[i].Data.Destination}\n";
                    ++mistakes;
                }
            }

            switch (isClosing)
            {
                case true when mistakes >= 1:
                {
                    string message = $"Oeps, je hebt {mistakes} fout(en) gemaakt!";
                    popupMenu.CreatePopup(message, Popup.Level.Warning);
                    Debug.Log(log);
                    return;
                }
                case false when mistakes < 1:
                {
                    popupMenu.CreatePopup("Je hebt alles op de goede plek!", Popup.Level.Info);
                    Complete();

                    foreach (Item item in GetComponentsInChildren<Item>())
                    {
                        item.OnEndDrag(null);
                        item.enabled = false;
                    }

                    return;
                }
            }
        }
    }
}