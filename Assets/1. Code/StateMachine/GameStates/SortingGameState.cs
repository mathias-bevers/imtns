using System;
using System.Collections.Generic;
using System.Linq;
using CleanRoom.InventorySystem;
using CleanRoom.Menus;
using CleanRoom.MiniGames.LockerMiniGame;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class SortingGameState : GameState
    {
        private const string FILENAME = "sorting_game.json";

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
                OnMistakeMade("Dat item hoort daar niet!");
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

            string json = SaveSystem.LoadFile(FILENAME);
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            JObject jObject = JObject.Parse(json);
            
            string[] locker = (jObject[nameof(InventoryItem.DestinationType.Locker)] as JArray)?.ToObject<string[]>();
            PopulateContainer(containers[InventoryItem.DestinationType.Locker].Grid.transform,
                GenerateItemList(locker));

            string[] trash = (jObject[nameof(InventoryItem.DestinationType.Trash)] as JArray)?.ToObject<string[]>();
            PopulateContainer(containers[InventoryItem.DestinationType.Trash].Grid.transform,
                GenerateItemList(trash));
        }

        private static InventoryItem[] GenerateItemList(string[] itemNames) =>
            itemNames.Select(Inventory.GetItemResource).ToArray();

        private void ValidateItems() => ValidateItems(true);

        private void ValidateItems(bool isClosing)
        {
            int mistakes = 0;
            JObject json = new();

            foreach (ItemContainer container in containers.Values)
            {
                Item[] contents = container.Grid.GetComponentsInChildren<Item>();
                for (int i = 0; i < contents.Length; ++i)
                {
                    if (container.Destination == contents[i].Data.Destination)
                    {
                        continue;
                    }

                    ++mistakes;
                }

                json.Add(container.Destination.ToString(), new JArray(contents.Select(i => i.Data.Name)));
            }

            SaveSystem.SaveFile(FILENAME, json.ToString());

            switch (isClosing)
            {
                case true when mistakes >= 1:
                {
                    string message = $"Oeps, je hebt {mistakes} fout(en) gemaakt!";
                    popupMenu.CreatePopup(message, Popup.Level.Warning);
                    return;
                }
                case false when mistakes < 1:
                {
                    popupMenu.CreatePopup("Je hebt alles op de goede plek!", Popup.Level.Info);
                    SaveSystem.SaveFile(FILENAME, string.Empty);
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