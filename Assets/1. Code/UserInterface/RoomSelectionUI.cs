using System;
using System.Text;
using CleanRoom.StateMachine;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

namespace CleanRoom.UserInterface
{
    public class RoomSelectionUI : MonoBehaviour
    {
        [SerializeField] private TransformStatePair<RoomState>[] transformStatePairs;

        private void OnEnable()
        {
            JObject gameStates = SaveSystem.LoadGameStates();

            for (int i = 0; i < transformStatePairs.Length; ++i)
            {
                TransformStatePair<RoomState> tsp = transformStatePairs[i];
                TextMeshProUGUI text = tsp.Transform.GetComponentInChildren<TextMeshProUGUI>();

                if (ReferenceEquals(null, tsp.State))
                {
                    text.SetText("Nog niet hier!");
                    continue;
                }

                int gameCount = 0;
                int completedGameCount = 0;
                int stars = 0;
                int roomMistakes = 0;

                foreach (string gameStateName in tsp.State.GetGameStateNames())
                {
                    JToken gameState = gameStates[gameStateName];
                    ++gameCount;
                    if (gameState["is_completed"].ToObject<bool>())
                    {
                        ++completedGameCount;
                    }

                    JArray array = gameState["feedback"] as JArray;
                    int mistakes = array.Count;
                    roomMistakes += mistakes;
                    stars += 3 - Mathf.Min(mistakes, 3);
                }

                StringBuilder builder = new();

                if (completedGameCount == gameCount)
                {
                    builder.Append("Score: ").Append(stars / gameCount).AppendLine();
                }
                else
                {
                    builder.AppendLine("Nog Niet Voltooid");
                }


                builder.Append("Mini Games Voltooid: ").Append(completedGameCount).Append('/').Append(gameCount)
                    .AppendLine();

                builder.Append("Fouten Gemaakt: ").AppendLine(roomMistakes.ToString());

                text.SetText(builder.ToString());
            }
        }


        [Serializable]
        public struct TransformStatePair<T> where T : State
        {
            [field: SerializeField] public Transform Transform { get; private set; }
            [field: SerializeField] public T State { get; private set; }
        }
    }
}