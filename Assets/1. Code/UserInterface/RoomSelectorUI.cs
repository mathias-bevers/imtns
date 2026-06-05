using System.Linq;
using System.Text;
using CleanRoom.Utils;
using KattenKasteel.FSM;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.UserInterface
{
    public class RoomSelectorUI : MonoBehaviour
    {
        private const string NOT_HERE_MESSAGE = "Nog niet hier...";
        private const string NOT_COMPLETED_MESSAGE = "Nog Niet Voltooid";

        private const string MINI_GAMES_COMPLETED = "Mini Games Voltooid: ";
        private const string MISTAKES_MADE = "Fouten Gemaakt: ";
        private const string SCORE = "Score: ";
        
        private const string STARS = "Stars";
        private const int MAX_STARS = 3;

        [SerializeField, ValidateInput("IsValidPairArray")]
        private SerializablePair<Button, State>[] roomSelectors;

        private void OnEnable()
        {
            if (!IsValidPairArray())
            {
                Debug.LogError("roomSelectors are not setup correctly...");
            }
            
            foreach (SerializablePair<Button, State> bsp in roomSelectors)
            {
                TextMeshProUGUI text = bsp.First.GetComponentInChildren<TextMeshProUGUI>();
                Transform starParent = bsp.First.transform.Find(STARS);

                if (ReferenceEquals(null, bsp.Second))
                {
                    text.SetText(NOT_HERE_MESSAGE);
                    starParent.gameObject.SetActive(false);
                    continue;
                }

                int stars = 0;
                int roomMistakes = 0;

                State[] children = bsp.Second.Children;
                int gameCount = children.Length;
                int completedGameCount = children.Count(child => child.IsCompleted);

                foreach (State child in children)
                {
                    string[] feedback = FeedbackLogger.GetFeedback(child.StateName);
                    stars += Mathf.Max(0, MAX_STARS - feedback.Length);
                    roomMistakes += feedback.Length;
                }

                StringBuilder builder = new();

                if (completedGameCount == gameCount)
                {
                    builder.AppendLine(SCORE);
                    int averageStars = gameCount == 0 ? 0 : stars / gameCount;
                    for (int ii = 0; ii < starParent.childCount; ++ii)
                    {
                        starParent.GetChild(ii).gameObject.SetActive(ii < averageStars);
                    }
                }
                else
                {
                    builder.AppendLine(NOT_COMPLETED_MESSAGE);
                    starParent.gameObject.SetActive(false);
                }


                builder.Append(MINI_GAMES_COMPLETED).Append(completedGameCount).Append('/').Append(gameCount)
                    .AppendLine();

                builder.Append(MISTAKES_MADE).AppendLine(roomMistakes.ToString());

                text.SetText(builder.ToString());
            }
        }

        private bool IsValidPairArray() // is used in validator attribute
        {
            foreach (SerializablePair<Button, State> bsp in roomSelectors)
            {
                if (ReferenceEquals(null, bsp))
                {
                    return false;
                }

                if (ReferenceEquals(null, bsp.Second))
                {
                    continue;
                }

                if (!bsp.Second.IsParent)
                {
                    return false;
                }
            }

            return true;
        }
    }
}