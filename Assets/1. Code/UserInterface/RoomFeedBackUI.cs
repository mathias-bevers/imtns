using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KattenKasteel.FSM;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace CleanRoom.UserInterface
{
    public class RoomFeedBackUI : MonoBehaviour
    {
        private const string PERFECT_SCORE = "Je hebt deze minigame perfect gedaan, goed bezig!";
        private const string EMPHASIS_OPEN = "<color=#A3F2CE>";
        private const string EMPHASIS_CLOSE = "</color>";

        [SerializeField] private RoomSelectorUI controller;
        [SerializeField, ValidateInput("IsRoomState")]
        private State roomState;
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            if (ReferenceEquals(null, roomState))
            {
                text.SetText("Deze kamer is nog niet klaar...");
                return;
            }

            if (!IsRoomState())
            {
                Debug.LogError("The state is not a room state");
                text.SetText("error...");
            }

            if (!roomState.IsCompleted)
            {
                text.SetText("Je hebt deze kamer nog niet voltooid!");
                return;
            }

            StringBuilder builder = new();

            foreach (string gameStateName in roomState.Children.Select(child => child.StateName))
            {
                string formattedStateName = gameStateName.Replace("State", string.Empty);
                // PascalCase -> Pascal Case
                formattedStateName = Regex.Replace(formattedStateName, "(\\B[A-Z])", " $1");
                builder.Append(EMPHASIS_OPEN).Append(formattedStateName).AppendLine(EMPHASIS_CLOSE);

                string[] feedback = FeedbackLogger.GetFeedback(gameStateName);
                builder.Append(feedback.Length == 0 ? PERFECT_SCORE : string.Join('\n', feedback));

                builder.AppendLine().AppendLine();
            }

            text.SetText(builder.ToString());
        }

        // Used as validator
        private bool IsRoomState() => roomState is { IsParent: true };
    }
}