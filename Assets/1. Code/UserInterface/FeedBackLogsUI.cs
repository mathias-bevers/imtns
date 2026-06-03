using System.Linq;
using System.Text.RegularExpressions;
using KattenKasteel.FSM;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace CleanRoom.UserInterface
{
    public class FeedBackLogsUI : MonoBehaviour
    {
        private const string ACCENT_HEX = "<color=#A3F2CE>";

        [SerializeField] private FeedbackUI controller;
        [SerializeField, ValidateInput("IsRoomState")] private State roomState;
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            if (ReferenceEquals(null, roomState))
            {
                text.SetText("Deze kamer is nog niet klaar...");
                return;
            }

            if (!roomState.IsCompleted)
            {
                text.SetText("Je hebt deze kamer nog niet voltooid!");
                return;
            }

            System.Text.StringBuilder builder = new();

            foreach (string gameStateName in roomState.Children.Select(child => child.Name))
            {
                string formattedStateName = gameStateName.Replace("State", string.Empty);
                // PascalCase -> Pascal Case
                formattedStateName = Regex.Replace(formattedStateName, "(\\B[A-Z])", " $1");
                builder.Append(ACCENT_HEX).Append(formattedStateName).AppendLine("</color>");

                string[] feedback = FeedbackLogger.GetFeedback(gameStateName);
                builder.Append(feedback.Length == 0
                    ? "Je hebt deze mini game perfect gedaan, goed bezig!"
                    : string.Join('\n', feedback));

                builder.AppendLine().AppendLine();
            }

            text.SetText(builder.ToString());
        }

        // Used as validator
        private bool IsRoomState() => roomState is { IsParent: true };
    }
}