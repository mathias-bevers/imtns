using CleanRoom.Menus;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class CleanItemMiniGameState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (!Player.Instance.Inventory.HasItem("Tablet"))
            {
                string message = "Zorg ervoor dat je de tablet bij je hebt!";
                MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(message, PopupMenu.Level.Warning);
                return;
            }

            SpawnDirt();
        }

        private void SpawnDirt()
        {
            
        }
    }
}