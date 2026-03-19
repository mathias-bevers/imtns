using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class CleanItemMiniGameState : GameState
    {
        [SerializeField] private GameObject interactables;
        [SerializeField] private Tablet tablet;

        public override void OnEnter()
        {
            base.OnEnter();
            interactables.SetActive(false);

            if (!Player.Instance.Inventory.HasItem("Tablet"))
            {
                string message = "Zorg ervoor dat je de tablet bij je hebt!";
                MenuManager.Instance.GetMenuOfType<PopupMenu>()
                    .CreatePopup(message, Popup.Level.Warning);
                return;
            }

            StartMiniGame();
        }

        private void StartMiniGame()
        {
            interactables.SetActive(true);
            tablet.SpawnDirt();
        }
    }
}