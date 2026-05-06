using CleanRoom.Inventory;
using CleanRoom.Menus;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.NewStateMachine
{
    public class SortingGameState : GameState
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        public int Mistakes { get; private set; }

        [SerializeField] private Item itemPrefab;
        private PopupMenu popupMenu;
        private Transform cleanRoomContainer;
        private ItemContainer[] containers;

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

            containers = GetComponentsInChildren<ItemContainer>(true);
            for (int i = 0; i < containers.Length; ++i)
            {
                containers[i].itemDroppedEvent += OnItemDropped;
            }

            LoadInventory();
        }

        private void OnItemDropped(bool isCorrect)
        {
            if (!isCorrect)
            {
                popupMenu.CreatePopup("dat was niet correct!", Popup.Level.Warning);
            }
            
            ValidateItems(false);
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

        private void ValidateItems() => ValidateItems(true);
        
        private void ValidateItems(bool isClosing)
        {
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

                    log += $"{children[ii].name}: {containers[i].Destination} != " +
                           $"{children[ii].Data.Destination}\n";
                    ++Mistakes;
                }
            }

            if (Mistakes < 1 && !isClosing)
            {
                popupMenu.CreatePopup("Je hebt alles op de goede plek!", Popup.Level.Info);
                Complete();
                
                foreach (Item item in GetComponentsInChildren<Item>())
                {
                    item.enabled = false;
                    item.OnEndDrag(null);
                }
                return;
            }

            if (!isClosing)
            {
                return;
            }
            
            string message = $"Oeps, je hebt {Mistakes} fout(en) gemaakt!";
            popupMenu.CreatePopup(message, Popup.Level.Warning);
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
            int cleanRoomIndex =
                GetContainerByDestination(InventoryItem.DestinationType.CleanRoom);

            if (cleanRoomIndex < 0)
            {
                Debug.LogError("could not find container for destination: clean room");
                return;
            }

            cleanRoomContainer = containers[cleanRoomIndex].Grid.transform;
        }
    }
}