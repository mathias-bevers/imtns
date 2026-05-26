using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class PopupMenu : Menu
    {
        public Popup Popup { get; private set; } = null;
        private readonly Queue<PopupInfo> queue = new();
        
        private readonly struct PopupInfo
        {
            public string Message { get;  } 
            public Popup.MessageType Type { get;  }
            public string Title { get; }

            public PopupInfo(string message, Popup.MessageType type, string title = null)
            {
                Message = message;
                Type = type;
                Title = string.IsNullOrEmpty(title) ? type.ToString() : title;
            }
        }
        

        private void Awake()
        {
            Popup = GetComponentInChildren<Popup>();
            
            Popup.closeEvent += OnPopupClose;
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

        [Button]
        private void CreateTestPopup()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            CreatePopup("This is a test", Popup.MessageType.Correct);
        }
    }
}