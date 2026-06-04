using System.Collections.Generic;
using KattenKasteel.FSM;
using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class PopupMenu : Menu
    {
        private const string STATE_COMPLETED_MESSAGE = "Je hebt deze mini-game voltooid!";
        
        public Popup Popup { get; private set; } = null;
        private readonly Queue<PopupInfo> queue = new();


        private void Awake()
        {
            Popup = GetComponentInChildren<Popup>();
            StateMachine.Instance.onStateCompleted += OnStateCompleted;

            Popup.closeEvent += (_) => OnPopupClose();
            Popup.Close();
        }

        private void OnPopupClose()
        {
            if (queue.Count < 1)
            {
                return;
            }

            ShowPopup();
        }

        public void CreatePopup(string message, Popup.MessageType type, string title = null)
        {
            queue.Enqueue(new PopupInfo(message, type, title));

            if (Popup.gameObject.activeInHierarchy)
            {
                return;
            }

            ShowPopup();
        }

        private void ShowPopup()
        {
            PopupInfo info = queue.Dequeue();
            Popup.Initialize(info.Message, info.Type, info.Title);
        }
        
        private void OnStateCompleted(State state)
        {
            if (state.IsParent)
            {
                return;
            }
            
            CreatePopup(STATE_COMPLETED_MESSAGE, Popup.MessageType.CompletedMiniGame, state.StateName);
        }

        private readonly struct PopupInfo
        {
            public Popup.MessageType Type { get; }
            public string Message { get; }
            public string Title { get; }

            public PopupInfo(string message, Popup.MessageType type, string title = null)
            {
                Message = message;
                Type = type;
                Title = string.IsNullOrEmpty(title) ? type.ToString() : title;
            }
        }
    }
}