using System.Text;
using CleanRoom.StateMachine;
using CleanRoom.Utils;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.UserInterface
{
    public class FeedbackUI : MonoBehaviour
    {
        private const string STARS = "Stars";
        
        public JObject GameStates { get; set; }
        
        [SerializeField] private SerializablePair<Button, RoomState>[] roomSelectors;
        

        private void OnEnable()
        {
            GameStates = SaveSystem.LoadGameStates();

            for (int i = 0; i < roomSelectors.Length; ++i)
            {
                SerializablePair<Button, RoomState> tsp = roomSelectors[i];
                TextMeshProUGUI text = tsp.First.GetComponentInChildren<TextMeshProUGUI>();
                Transform starParent = tsp.First.transform.Find(STARS);

                if (ReferenceEquals(null, tsp.Second))
                {
                    text.SetText("Nog niet hier!");
                    starParent.gameObject.SetActive(false);
                    continue;
                }

                int gameCount = 0;
                int completedGameCount = 0;
                int stars = 0;
                int roomMistakes = 0;

                foreach (string gameStateName in tsp.Second.GetGameStateNames())
                {
                    JToken gameState = GameStates[gameStateName];

                    if (ReferenceEquals(null, gameState))
                    {
                        Debug.LogError("could not find record for: " + gameStateName);
                        continue;
                    }

                    ++gameCount;
                    if (gameState["is_completed"]!.ToObject<bool>())
                    {
                        ++completedGameCount;
                    }

                    if (gameState["feedback"] is not JArray array)
                    {
                        Debug.LogError("could not find feedback for: " + gameStateName);
                        continue;
                    }
                    
                    tsp.First.onClick.RemoveAllListeners();
                    
                    int mistakes = array.Count;
                    roomMistakes += mistakes;
                    stars += 3 - Mathf.Min(mistakes, 3);
                }

                StringBuilder builder = new();

                if (completedGameCount == gameCount)
                {
                    builder.AppendLine("Score: ");
                    int averageStars = stars / gameCount;
                    for (int ii = 0; ii < starParent.childCount; ++ii)
                    {
                        starParent.GetChild(ii).gameObject.SetActive(ii < averageStars);
                    }
                }
                else
                {
                    builder.AppendLine("Nog Niet Voltooid");
                    starParent.gameObject.SetActive(false);
                }


                builder.Append("Mini Games Voltooid: ").Append(completedGameCount).Append('/').Append(gameCount)
                    .AppendLine();

                builder.Append("Fouten Gemaakt: ").AppendLine(roomMistakes.ToString());

                text.SetText(builder.ToString());
            }
        }
    }
}