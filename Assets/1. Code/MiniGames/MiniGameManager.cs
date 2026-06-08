using CleanRoom.Menus;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.MiniGames
{
    [RequireComponent(typeof(Transitioner))]
    public abstract class MiniGameManager<T> : Singleton<T> where T : Singleton<T>
    {
        public Transitioner OnCompletionTransition { get; private set; }
        public GameManager GameManager { get; private set; }
        public string StateName { get; private set; }

        public override void Awake()
        {
            base.Awake();

            OnCompletionTransition = GetComponent<Transitioner>();
            GameManager = GameManager.Instance;
            StateName = StateMachine.Instance.ActiveState.StateName;
            StartMiniGame();
        }

        protected abstract void StartMiniGame();

        public void CompleteMiniGame()
        {
            PopupManager.Instance.Popup.closeEvent += OnPopupClose;
            StateMachine.Instance.CompleteActiveState();
        }

        private void OnPopupClose(Popup.MessageType messageType)
        {
            if (messageType != Popup.MessageType.CompletedMiniGame)
            {
                return;
            }

            PopupManager.Instance.Popup.closeEvent -= OnPopupClose;
            OnCompletionTransition.Transition();
        }
    }
}