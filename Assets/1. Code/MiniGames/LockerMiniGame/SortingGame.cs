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
        private const string TABLET_NAME = "Tablet";
        private static readonly Dictionary<InventoryItem.DestinationType, string> NAME_MAP = new()
        {
            { InventoryItem.DestinationType.Locker, "kluis" },
            { InventoryItem.DestinationType.CleanRoom, "clean room" },
            { InventoryItem.DestinationType.Trash, "prullen bak" }
        };

        [SerializeField] private GameObject tabletOnTray;
        [SerializeField] private Transitioner onCompleteTransition;
        [SerializeField] private InventoryContents contents;
        private DropZone[] dropZones = Array.Empty<DropZone>();
        private int currentItem = -1;
        private Inventory inventory = null;
        private SortingItem sortingItem = null;

        private string stateName;

        private void OnEnable()
        {
            inventory = new Inventory(contents);
            StartMiniGame();
        }

        private void StartMiniGame()
        {
            stateName = StateMachine.Instance.ActiveState.StateName;

            dropZones = GetComponentsInChildren<DropZone>(true);
            sortingItem = GetComponentInChildren<SortingItem>();

            for (int i = 0; i < dropZones.Length; ++i)
            {
                dropZones[i].itemDroppedEvent += OnItemDropped;
            }

            tabletOnTray.SetActive(false);
            sortingItem.SetActive(true);

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
            
            FlashOverlay.Instance.PlayAnimation(true);

            if (string.Equals(TABLET_NAME, item.name))
            {
                tabletOnTray.SetActive(true);
            }

            NextItem();
        }

        private void NextItem()
        {
            ++currentItem;
            
            if (currentItem != inventory.Size)
            {
                sortingItem.DisplayData(inventory.GetItemAtIndex(currentItem));
                return;
            }

            sortingItem.SetActive(false);
            StateMachine.Instance.CompleteActiveState();
            PopupManager.Instance.Popup.closeEvent += OnCompletePopupClose;
        }

        private void OnCompletePopupClose(Popup.MessageType messageType)
        {
            if (messageType != Popup.MessageType.CompletedMiniGame)
            {
                return;
            }

            onCompleteTransition.Transition();
            PopupManager.Instance.Popup.closeEvent -= OnCompletePopupClose;
        }
    }
}