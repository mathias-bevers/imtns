using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;

namespace CleanRoom.NewStateMachine
{
    public class CleanGameState : GameState
    {
        public override bool CanEnter => Player.Instance.Inventory.HasItem("Tablet") ||
                                         !IsCompleted;

        [field: SerializeField] public DragAndSnap Wipe { get; private set; }
        [field: SerializeField] public Tablet Tablet { get; private set; }
        [field: SerializeField] public ConditionalOnClick WipeBox { get; private set; }
        [field: SerializeField] public ConditionalOnClick BagDispenser { get; private set; }

        [SerializeField] private GameObject interactables;


        protected void OnEnable()
        {
            EnterEvent.AddListener(StartMiniGame);
            ExitEvent.AddListener(ValidateCleanliness);
        }

        protected void OnDisable()
        {
            EnterEvent.RemoveAllListeners();
            ExitEvent.RemoveAllListeners();
        }

        private void StartMiniGame()
        {
            interactables.SetActive(true);
            Tablet.SpawnDirt();
        }

        private void ValidateCleanliness()
        {
            int mistakes = Tablet.GetComponentsInChildren<DirtPiece>().Length;

            if (mistakes == 0)
            {
                return;
            }

            string message = $"Oeps, je hebt {mistakes} fout(en) gemaakt!";
            MenuManager.Instance.GetMenuOfType<PopupMenu>()
                .CreatePopup(message, Popup.Level.Warning);
        }
    }
}