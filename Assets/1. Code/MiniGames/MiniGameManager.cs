using CleanRoom.Menus;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.MiniGames
{
    [RequireComponent(typeof(Transitioner))]
    public abstract class MiniGameManager<T> : Singleton<T> where T : Singleton<T>
    {
        [SerializeField, TextArea] private string startMessage;
        [SerializeField, TextArea] private string endMessage;
        
        public Transitioner OnCompletionTransition { get; private set; }
        public GameManager GameManager { get; private set; }
        public string StateName { get; private set; }

        private bool isCompleted;

        public override void Awake()
        {
            base.Awake();

            OnCompletionTransition = GetComponent<Transitioner>();
            GameManager = GameManager.Instance;
            StateName = StateMachine.Instance.ActiveState.StateName;
            isCompleted = false;
            
            PopupManager.Instance.CreatePopup(startMessage, Popup.MessageType.StartMiniGame, StateName);
            StartMiniGame();
        }

        protected abstract void StartMiniGame();

        public void CompleteMiniGame()
        {
            if (isCompleted)
            {
                return;
            }
            SoundManager.Instance.PlaySFX("minigamecomplete");
            isCompleted = true;
            
            
            if (StateMachine.Instance.ActiveState.IsParent)
            {
                Debug.LogError("Mini Game Managers should only be present in non parent states.");
                return;
            }
            
            int stars = Mathf.Max(0, 3 - FeedbackLogger.GetFeedback(StateName).Length);
            PopupManager popupManager = PopupManager.Instance;
            
            popupManager.CreatePopup(stars + endMessage, Popup.MessageType.CompletedMiniGame, StateName);
            popupManager.Popup.closeEvent += OnPopupClose;
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