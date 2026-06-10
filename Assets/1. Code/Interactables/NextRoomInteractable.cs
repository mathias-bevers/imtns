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
        private const string THANKS_FOR_PLAYING = "Bedankt voor het spelen";
        private const string END_OF_GAME = "Deze game is nog in development, voor nu is dit het einde!" +
                                           " Je gaat nu terug naar het hoofd menu.";

        [SerializeField] private Condition condition;
        [SerializeField] private Transitioner transitioner;
        [SerializeField, TextArea] private string notReadyBody;

        private bool isEndPopup;

        public void Interact()
        {
            PopupManager popupManager = PopupManager.Instance;

            if (!condition.IsSatisfied(null))
            {
                popupManager.CreatePopup(notReadyBody, Popup.MessageType.Incorrect, NOT_READY_TITLE);
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

            popupManager.CreatePopup(stars.ToString() + builder, Popup.MessageType.Feedback,
                string.Concat(activeState.StateName, FEEDBACK));
            popupManager.Popup.closeEvent += OnPopupClose;
        }

        private void OnPopupClose(Popup.MessageType messageType)
        {
            if (messageType != Popup.MessageType.Feedback && isEndPopup)
            {
                transitioner.Transition();
                return;
            }

            int availableRooms = StateMachine.Instance.GetStates(s => s.IsParent && !s.IsCompleted).Length;
            if (availableRooms == 0)
            {
                isEndPopup = true;
                PopupManager.Instance.CreatePopup(END_OF_GAME, Popup.MessageType.Correct, THANKS_FOR_PLAYING);
                return;
            }

            PopupManager.Instance.Popup.closeEvent -= OnPopupClose;
            transitioner.Transition();
        }
    }
}