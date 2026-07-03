using System;
using CleanRoom.InventorySystem;
using CleanRoom.Menus;
using UnityEngine;

namespace CleanRoom.MiniGames.LockerMiniGame
{
    public class SortingGame : MiniGameManager<SortingGame>
    {
        private const string TABLET_NAME = "Tablet";

        [SerializeField] private GameObject tabletOnTray;
        [SerializeField] private InventoryContents contents;


        private DropZone[] dropZones = Array.Empty<DropZone>();
        private int currentItem = -1;
        private Inventory inventory = null;
        private SortingItem sortingItem = null;


        protected override void StartMiniGame()
        {
            inventory = new Inventory(contents);

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
                GameManager.OnMistakeMade(StateName, item.IncorrectMessage);
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
            CompleteMiniGame();
        }
    }
}