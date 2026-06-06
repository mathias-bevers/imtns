using CleanRoom.Menus;
using CleanRoom.Utils;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.Interactables
{
    public class NextRoomInteractable : MonoBehaviour
    {
        private const string FEEDBACK = " feedback";
        private const string PERFECT_SCORE = "Je hebt deze minigame perfect gedaan, goed bezig!";
        private const string ACCENT_HEX = "<color=#A3F2CE>";
        private const string CLOSE_COLOR = "</color>";
        private const int MAX_STARS = 3;

        private const string NOT_READY_TITLE = "Je bent nog niet klaar!";
        [SerializeField] private Condition condition;
        [SerializeField, TextArea] private string notReadyBody;

        public void Interact()
        {
            PopupMenu popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>(true);
            
            if (!condition.IsSatisfied(null))
            {
                popupMenu.CreatePopup(notReadyBody, Popup.MessageType.Incorrect, NOT_READY_TITLE);
                return;
            }

            System.Text.StringBuilder builder = new();
            int totalEarnedStars = 0;
            State activeState = StateMachine.Instance.ActiveState;
            State[] activeStateChildren = activeState.Children;

            foreach (State child in activeStateChildren)
            {
                string[] feedback = FeedbackLogger.GetFeedback(child.StateName);
                
                builder.Append(ACCENT_HEX).Append(child.StateName).AppendLine(CLOSE_COLOR);
                builder.Append(feedback.IsNullOrEmpty() ? PERFECT_SCORE : string.Join('\n', feedback));
                builder.Append("\n\n");
                
                totalEarnedStars += Mathf.Max(0, MAX_STARS - feedback.Length);
            }

            int stars = totalEarnedStars / activeStateChildren.Length;
            
            popupMenu.CreatePopup(stars.ToString() + builder, Popup.MessageType.Feedback,
                string.Concat(activeState.StateName, FEEDBACK));
        }
    }
}