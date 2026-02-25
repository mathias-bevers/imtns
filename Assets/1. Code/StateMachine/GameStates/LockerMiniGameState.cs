using CleanRoom.Inventory;
using CleanRoom.MiniGames.LockerMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class LockerMiniGameState : GameState
    {
        [SerializeField] private Transform inventoryGrid;
        [SerializeField] private LockerItem lockerItemPrefab;

        public override void OnEnter()
        {
            base.OnEnter();
            LoadInventory();
        }

        private void LoadInventory()
        {
            InventoryItem[] inventory = Player.Instance.Inventory.GetItems();

            inventoryGrid.DestroyAllChildren();

            for (int i = 0; i < inventory.Length; ++i)
            {
                LockerItem lockerItem = Instantiate(lockerItemPrefab, inventoryGrid);
                lockerItem.Setup(inventory[i]);
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