using CleanRoom.Menus;
using CleanRoom.MiniGames.CleanMiniGame;
using UnityEngine;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class CleanGame : Singleton<CleanGame>
    {
        private const string NOT_CLEAN_MESSAGE = "Oeps, de tablet was nog niet helemaal school!";

        [field: SerializeField] public DragAndSnap Wipe { get; private set; }
        [field: SerializeField] public Tablet Tablet { get; private set; }
        [field: SerializeField] public ConditionalOnClick WipeBox { get; private set; }
        [field: SerializeField] public ConditionalOnClick BagDispenser { get; private set; }

        [SerializeField] private GameObject interactables;

        protected void OnEnable()
        {
            StartMiniGame();
        }

        protected void OnDisable()
        {
            ValidateCleanliness();
        }

        private void StartMiniGame()
        {
            interactables.SetActive(true);
            Tablet.SpawnDirt();
            foreach (DirtPiece dirtPiece in Tablet.GetComponentsInChildren<DirtPiece>())
            {
                dirtPiece.mistakeMadeEvent += GameManager.Instance.OnMistakeMade;
            }
        }

        private void ValidateCleanliness()
        {
            if (Tablet.GetComponentsInChildren<DirtPiece>().Length == 0)
            {
                return;
            }
            
            MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(NOT_CLEAN_MESSAGE, Popup.MessageType.Incorrect);
        }
    }
}