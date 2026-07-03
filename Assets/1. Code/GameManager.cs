using System;
using CleanRoom.Menus;

namespace CleanRoom
{
    public class GameManager : Singleton<GameManager>
    {
        private static readonly TimeSpan MISTAKE_TIME_OUT = new(0, 0, 2); // two second delay
        private DateTime lastMistakeTime;

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }

        public void OnMistakeMade(string stateName, string message)
        {
            if (DateTime.Now - lastMistakeTime < MISTAKE_TIME_OUT)
            {
                return;
            }

            lastMistakeTime = DateTime.Now;

            PopupManager.Instance.CreatePopup(message, Popup.MessageType.Incorrect);
            FeedbackLogger.AddFeedback(stateName, message);
        }
    }
}