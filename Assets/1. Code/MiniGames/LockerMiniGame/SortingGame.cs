using System;
using System.Collections.Generic;
using CleanRoom.InventorySystem;
using CleanRoom.Menus;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class SortingGame : Singleton<SortingGame>
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

        private string stateName;
        private DropZone[] dropZones = Array.Empty<DropZone>();
        private int currentItem = -1;
        private Inventory inventory = null;
        private PopupMenu popupMenu = null;
        private SortingItem sortingItem = null;

        private void OnEnable()
        {
            StartMiniGame();
        }

        private void StartMiniGame()
        {
            stateName = StateMachine.Instance.ActiveState.Name;
            
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
                GameManager.Instance.OnMistakeMade(stateName, item.IncorrectMessage);
                return;
            }
            
            popupMenu.CreatePopup(item.CorrectMessage, Popup.MessageType.Correct);

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
                StateMachine.Instance.CompleteActiveState();
                return;
            }

            sortingItem.Setup(inventory.GetItemAtIndex(currentItem));
        }
    }
}