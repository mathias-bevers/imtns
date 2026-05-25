using System;
using System.Text.RegularExpressions;
using CleanRoom.StateMachine;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

namespace CleanRoom.UserInterface
{
    public class FeedBackLogsUI : MonoBehaviour
    {
        private const string ACCENT_HEX = "<color=#A3F2CE>";
        
        [SerializeField] private FeedbackUI controller;
        [SerializeField] private RoomState roomState;
        [SerializeField] private TextMeshProUGUI text;
        
        private void OnEnable()
        {
            if (ReferenceEquals(null, roomState))
            {
                text.SetText("Deze kamer is nog niet klaar...");
                return;
            }

            if (!controller.IsRoomStateCompleted(roomState.StateName))
            {
                text.SetText("Je hebt deze kamer nog niet voltooid!");
                return;
            }

            System.Text.StringBuilder builder = new();

            foreach (string gameStateName in roomState.GetGameStateNames())
            {
                string formattedStateName = gameStateName.Replace("State", string.Empty);
                // PascalCase -> Pascal Case
                formattedStateName = Regex.Replace(formattedStateName, "(\\B[A-Z])", " $1"); 
                builder.Append(ACCENT_HEX).Append(formattedStateName).AppendLine("</color>");

                if (controller.GameStates[gameStateName]["feedback"] is not JArray array)
                {
                    Debug.LogError("could not find feedback for: " + gameStateName);
                }
                else if (array.Count == 0)
                {
                    builder.Append("Je hebt deze mini game perfect gedaan, goed bezig!");
                }
                else
                {
                    builder.Append(string.Join('\n', array));
                }
                
                builder.AppendLine().AppendLine();
            }
            
            text.SetText(builder.ToString());
        }
    }
}