using CleanRoom.Menus;
using CleanRoom.StateMachine;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CleanRoom.Interactables
{
    public class RoomStateSwitcher : MonoBehaviour
    {
        private const string ACCENT_HEX = "<color=#A3F2CE>";
        private const int MAX_STARS = 3;

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

            (string title, string text) feedback = GetFeedBack();
            popupMenu.CreatePopup(feedback.text, Popup.MessageType.Feedback, feedback.title);
            popupMenu.Popup.closeEvent += OnPopupClose;
        }

        private void OnPopupClose()
        {
            stateMachine.GoToNextRoom();
            popupMenu.Popup.closeEvent -= OnPopupClose;
        }

        private (string title, string text) GetFeedBack()
        {
            RoomState roomState = StateMachine.StateMachine.Instance.ActiveState as RoomState;
            string[] gameStateNames = roomState?.GetGameStateNames();
            if (ReferenceEquals(null, gameStateNames) || gameStateNames.Length < 1)
            {
                return ("ERROR", $"Could not load game state names for: {roomState?.name}");
            }

            System.Text.StringBuilder builder = new();
            JObject jObject = SaveSystem.LoadGameStates();
            int totalStars = 0;

            foreach (string gameStateName in gameStateNames)
            {
                JToken token = jObject[gameStateName];
                if (ReferenceEquals(null, token))
                {
                    Debug.LogError("could not find record for: " + gameStateName);
                    continue;
                }

                if (token["feedback"] is not JArray array)
                {
                    Debug.LogError("could not find feedback for: " + gameStateName);
                    continue;
                }

                totalStars += Mathf.Max(0, MAX_STARS - array.Count);

                builder.Append(ACCENT_HEX).Append(gameStateName).AppendLine("</color>");
                builder.Append(array.Count == 0
                    ? "Je hebt deze mini game perfect gedaan, goed bezig!"
                    : string.Join('\n', array));

                builder.AppendLine().AppendLine();
            }

            int averageStars = totalStars / gameStateNames.Length;
            return (roomState.StateName, averageStars.ToString() + builder);
        }
    }
}