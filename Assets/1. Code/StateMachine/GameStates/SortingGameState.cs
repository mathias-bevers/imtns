using System;
using System.Collections.Generic;
using CleanRoom.InventorySystem;
using CleanRoom.Menus;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class SortingGameState : GameState
    {
        private const string FILENAME = "sorting_game.json";
        private const string TABLET_NAME = "Tablet";
        private static readonly Dictionary<InventoryItem.DestinationType, string> NAME_MAP = new()
        {
            { InventoryItem.DestinationType.Locker, "kluis" },
            { InventoryItem.DestinationType.CleanRoom, "clean room" },
            { InventoryItem.DestinationType.Trash, "prullen bak" }
        };

        [SerializeField] private GameObject tabletOnTray;

        private DropZone[] dropZones = Array.Empty<DropZone>();
        private int currentItem = -1;
        private Inventory inventory = null;
        private PopupMenu popupMenu = null;
        private SortingItem sortingItem = null;

        private void OnEnable()
        {
            EnterEvent.AddListener(StartMiniGame);
        }

        private void OnDisable()
        {
            EnterEvent.RemoveListener(StartMiniGame);
        }

        private void StartMiniGame()
        {
            popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>();
            dropZones = GetComponentsInChildren<DropZone>(true);
            sortingItem = GetComponentInChildren<SortingItem>();
            inventory = Player.Instance.Inventory;

            for (int i = 0; i < dropZones.Length; ++i)
            {
                dropZones[i].itemDroppedEvent += OnItemDropped;
            }

            tabletOnTray.SetActive(false);
            sortingItem.Image.enabled = true;

            currentItem = -1;
            NextItem();
        }

        private void OnItemDropped(InventoryItem item, InventoryItem.DestinationType dropZoneType)
        {
            bool isCorrect = item.Destination == dropZoneType;

            if (!isCorrect)
            {
                OnMistakeMade(string.Concat("Een ", item.Name, " hoort niet in de " + NAME_MAP[dropZoneType], '.'));
                return;
            }


            popupMenu.CreatePopup("Dat was correct!", Popup.MessageType.Correct);

            if (string.Equals(TABLET_NAME, item.name))
            {
                tabletOnTray.SetActive(true);
            }

            NextItem();
        }

        private void NextItem()
        {
            ++currentItem;
            if (currentItem == inventory.Size)
            {
                sortingItem.Image.enabled = false;
                Complete();
                return;
            }

            sortingItem.Setup(inventory.GetItemAtIndex(currentItem));
        }
    }
}