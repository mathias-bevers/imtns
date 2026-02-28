using NaughtyAttributes;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class PopupMenu : Menu
    {
        [SerializeField] private Popup popupPrefab;
        [SerializeField] private Transform popupParent;

        public void CreatePopup(string message, Popup.Level level)
        {
            Popup popup = Instantiate(popupPrefab, popupParent);
            popup.Initialize(message, level);
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