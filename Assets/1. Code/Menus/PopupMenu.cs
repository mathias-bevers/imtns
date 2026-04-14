using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class PopupMenu : Menu
    {
        [SerializeField] private Popup popupPrefab;
        [SerializeField] private Transform popupParent;

        private readonly HashSet<string> popups = new();

        public void CreatePopup(string message, Popup.Level level)
        {
            if (!popups.Add(message))
            {
                return;
            }

            Popup popup = Instantiate(popupPrefab, popupParent);
            popup.Initialize(message, level);
            popup.destroyEvent += OnPopupDestroy;
        }

        private void OnPopupDestroy(string message)
        {
            popups.Remove(message);
        }

        [Button]
        private void CreateTestPopup()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            CreatePopup("This is a test", Popup.Level.Info);
        }
    }
}