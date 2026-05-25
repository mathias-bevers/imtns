using CleanRoom.Menus;
using UnityEngine;

namespace CleanRoom.Interactables
{
    public class RoomStateSwitcher : MonoBehaviour
    {
        private PopupMenu popupMenu = null;
        private StateMachine.StateMachine stateMachine = null;

        public void RequestNextRoom()
        {
            stateMachine = StateMachine.StateMachine.Instance;
            popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>();

            if (!stateMachine.IsStateCompleted(stateMachine.ActiveState.StateName))
            {
                popupMenu.CreatePopup("Je hebt nog niet alle minigames in deze kamer voltooid.",
                    Popup.MessageType.Incorrect, "Nog niet klaar!");
                return;
            }
            
            // menu.CreatePopup(); //TODO: get feedback of current room.
            popupMenu.Popup.closeEvent += OnPopupClose;
        }

        private void OnPopupClose()
        {
            stateMachine.GoToNextRoom();
            popupMenu.Popup.closeEvent -= OnPopupClose;
        }

        private string GetFeedBack()
        {
            
        }
    }
}