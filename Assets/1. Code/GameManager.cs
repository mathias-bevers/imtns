using System;
using CleanRoom.Menus;
using UnityEngine;

namespace CleanRoom
{
    public class GameManager : Singleton<GameManager>
    {
        private static readonly TimeSpan MISTAKE_TIME_OUT = new(0, 0, 2); // two second delay
        private DateTime lastMistakeTime;
        private PopupMenu popupMenu = null;
        
        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();

            popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>();
        }

        public void OnMistakeMade(string stateName, string message)
        {
            if ((DateTime.Now - lastMistakeTime) < MISTAKE_TIME_OUT)
            {
                return;
            }

            lastMistakeTime = DateTime.Now;
            
            if (ReferenceEquals(null, popupMenu))
            {
                Debug.LogError("popup menu is null\nmessage:" + message);
                return;
            }
            
            popupMenu.CreatePopup(message, Popup.MessageType.Incorrect);
            FeedbackLogger.AddFeedback(stateName, message);
        }
    }
}