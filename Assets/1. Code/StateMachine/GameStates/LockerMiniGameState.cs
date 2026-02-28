using System;
using CleanRoom.Inventory;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class LockerMiniGameState : GameState
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        public int Mistakes { get; private set; }
        
        
        [SerializeField] private Item itemPrefab;
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

            int cleanRoomIndex = GetContainerByDestination(InventoryItem.DestinationType.CleanRoom);
            
            if (cleanRoomIndex < 0)
            {
                Debug.LogError("could not find container for destination: clean room");
                return;
            }

            Transform gridTransform = containers[cleanRoomIndex].Grid.transform;
            gridTransform.DestroyAllChildren();
            
            for (int i = 0; i < inventory.Length; ++i)
            {
                Item item = Instantiate(itemPrefab, gridTransform);
                item.Setup(inventory[i]);
            }
        }

        private void ValidateItems()
        {
            Mistakes = 0;
            
            for (int i = 0; i < containers.Length; ++i)
            {
                Item[] children = containers[i].Grid.GetComponentsInChildren<Item>();
                for (int ii = 0; ii < children.Length; ++ii)
                {
                    if (containers[i].Destination == children[ii].Data.Destination)
                    {
                        continue;
                    }

                    ++Mistakes;
                }
            }
            
            Debug.Log("mistakes: " + Mistakes);
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
    }
}