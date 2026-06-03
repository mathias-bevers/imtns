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
    public class FeedbackUI : MonoBehaviour
    {
        private const string STARS = "Stars";
        private const int MAX_STARS = 3;

        [SerializeField, ValidateInput("IsValidPairArray")]
        private SerializablePair<Button, State>[] roomSelectors;

        private void OnEnable()
        {
            foreach (SerializablePair<Button, State> bsp in roomSelectors)
            {
                TextMeshProUGUI text = bsp.First.GetComponentInChildren<TextMeshProUGUI>();
                Transform starParent = bsp.First.transform.Find(STARS);

                if (ReferenceEquals(null, bsp.Second))
                {
                    text.SetText("Nog niet hier!");
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
                    string[] feedback = FeedbackLogger.GetFeedback(child.name);
                    stars += Mathf.Max(0, MAX_STARS - feedback.Length);
                    roomMistakes += feedback.Length;
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

        private bool IsValidPairArray()
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