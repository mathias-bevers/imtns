using System;
using CleanRoom.Inventory;
using CleanRoom.Menus;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class LockerMiniGameState : GameState
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        public int Mistakes { get; private set; }
        
        
        [SerializeField] private Item itemPrefab;
        private Transform cleanRoomContainer;
        private ItemContainer[] containers;


        public override void Initialize()
        {
            containers = GetComponentsInChildren<ItemContainer>(true);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            LoadInventory();
        }

        public override void OnExit()
        {
            base.OnExit();
            ValidateItems();
        }

        private void LoadInventory()
        {
            InventoryItem[] inventory = Player.Instance.Inventory.GetInventory();

            if (ReferenceEquals(null, cleanRoomContainer))
            {
                SetCleanRoomContainer();
            }
            
            cleanRoomContainer.DestroyAllChildren();
            
            for (int i = 0; i < inventory.Length; ++i)
            {
                Item item = Instantiate(itemPrefab, cleanRoomContainer);
                item.Setup(inventory[i]);
            }
        }

        private void ValidateItems()
        {
            // the called by the initial on exit which does not need to validate.
            if (ReferenceEquals(null, cleanRoomContainer))
            {
                return;
            }
            
            Mistakes = 0;
            string log = string.Empty;
            
            for (int i = 0; i < containers.Length; ++i)
            {
                Item[] children = containers[i].Grid.GetComponentsInChildren<Item>();
                for (int ii = 0; ii < children.Length; ++ii)
                {
                    if (containers[i].Destination == children[ii].Data.Destination)
                    {
                        continue;
                    }

                    log += $"{children[ii].name}: {containers[i].Destination} != {children[ii].Data.Destination}";
                    ++Mistakes;
                }
            }

            if (Mistakes < 1)
            {
                return;
            }

            string message = $"Oeps, je hebt {Mistakes} fout(en) gemaakt!";
            MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(message, Popup.Level.Warning);
            Debug.Log(log);
        }

        private int GetContainerByDestination(InventoryItem.DestinationType destination)
        {
            for (int i = 0; i < containers.Length; ++i)
            {
                if (destination != containers[i].Destination)
                {
                    continue;
                }

                return i;
            }

            return -1;
        }

        private void SetCleanRoomContainer()
        {
            int cleanRoomIndex = GetContainerByDestination(InventoryItem.DestinationType.CleanRoom);
            
            if (cleanRoomIndex < 0)
            {
                Debug.LogError("could not find container for destination: clean room");
                return;
            }

            cleanRoomContainer = containers[cleanRoomIndex].Grid.transform;
        }
    }
}