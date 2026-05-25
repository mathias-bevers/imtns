using System;
using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class PopupMenu : Menu
    {
        [SerializeField] private Popup popup;

        private void Awake()
        {
            popup.Close();
        }

        public void CreatePopup(string message, Popup.MessageType type, string title = null)
        {
            if (popup.gameObject.activeInHierarchy)
            {
                return;
            }

            popup.Initialize(message, type, title);
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