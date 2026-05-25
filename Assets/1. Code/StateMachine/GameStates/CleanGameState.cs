using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class CleanGameState : GameState
    {
        private const string NOT_CLEAN_MESSAGE = "Oeps, de tablet was nog niet helemaal school!";

        [field: SerializeField] public DragAndSnap Wipe { get; private set; }
        [field: SerializeField] public Tablet Tablet { get; private set; }
        [field: SerializeField] public ConditionalOnClick WipeBox { get; private set; }
        [field: SerializeField] public ConditionalOnClick BagDispenser { get; private set; }

        [SerializeField] private GameObject interactables;
        public override bool CanEnter =>
            base.CanEnter && Player.Instance.Inventory.HasItem("Tablet");

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
            foreach (DirtPiece dirtPiece in Tablet.GetComponentsInChildren<DirtPiece>())
            {
                dirtPiece.mistakeMade += OnMistakeMade;
            }
        }

        private void ValidateCleanliness()
        {
            if (Tablet.GetComponentsInChildren<DirtPiece>().Length == 0)
            {
                return;
            }
            
            MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(NOT_CLEAN_MESSAGE, Popup.Level.Warning);
        }
    }
}