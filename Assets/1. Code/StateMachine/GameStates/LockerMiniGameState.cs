using CleanRoom.Inventory;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class LockerMiniGameState : GameState
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        
        [SerializeField] private Transform inventoryPanel;
        [SerializeField] private Item itemPrefab;

        private Transform inventoryGrid;

        public override void OnEnter()
        {
            base.OnEnter();
            inventoryGrid ??= inventoryPanel.GetComponentInChildren<UnityEngine.UI.GridLayoutGroup>().transform;
            LoadInventory();
        }

        private void LoadInventory()
        {
            InventoryItem[] inventory = Player.Instance.Inventory.GetInventory();

            inventoryGrid.DestroyAllChildren();

            for (int i = 0; i < inventory.Length; ++i)
            {
                Item item = Instantiate(itemPrefab, inventoryGrid);
                item.Setup(inventory[i]);
            }
        }


        private void OnGUI()
        {
            if (!IsActive)
            {
                return;
            }

            if (GUI.Button(new Rect(10, 10, 200, 150), GetType().Name + ": Return to roaming state"))
            {
                GameStateController.Instance.SwitchToState<RoamingGameState>();
            }
        }
    }
}