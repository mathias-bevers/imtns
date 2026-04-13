using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine.GameStates
{
    public class CleanItemMiniGameState : GameState
    {
        [field: SerializeField] public DragAndSnap Wipe { get; private set; }
        [SerializeField] private GameObject interactables;
        [SerializeField] private Tablet tablet;

        protected override void OnEnter()
        {
            interactables.SetActive(false);

            if (!Player.Instance.Inventory.HasItem("Tablet"))
            {
                string message = "Zorg ervoor dat je de tablet bij je hebt!";
                MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(message, Popup.Level.Warning);
                return;
            }

            StartMiniGame();
        }

        protected override void OnExit()
        {
            ValidateCleanliness();
            base.OnExit();
        }

        private void StartMiniGame()
        {
            interactables.SetActive(true);
            tablet.SpawnDirt();
        }

        private void ValidateCleanliness()
        {
            int mistakes = tablet.GetComponentsInChildren<DirtPiece>().Length;

            if (mistakes == 0)
            {
                return;
            }
            
            string message = $"Oeps, je hebt {mistakes} fout(en) gemaakt!";
            MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(message, Popup.Level.Warning);
        }
    }
}