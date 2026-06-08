using KattenKasteel.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class CleanGame : MiniGameManager<CleanGame>
    {
        [field: SerializeField] public DragAndSnap Wipe { get; private set; }
        [field: SerializeField] public Tablet Tablet { get; private set; }
        [field: SerializeField] public Button WipeBox { get; private set; }
        [field: SerializeField] public Button BagDispenser { get; private set; }

        [SerializeField] private CleaningErrors[] errors;
        [SerializeField] private Transform toolContainer; 
        [SerializeField] private GameObject interactables;

        protected override void StartMiniGame()
        {
            interactables.SetActive(true);
        }

        public void ShowTool(GameObject tool)
        {
            for (int i = 0; i < toolContainer.childCount; ++i)
            {
                toolContainer.GetChild(i).gameObject.SetActive(false);
            }
            
            tool.gameObject.SetActive(true);
        }
        
        public void ShowError(Tablet.CleanlinessLevel level, bool isTooEarly)
        {
            CleaningErrors error = null;
            for (int i = 0; i < errors.Length; ++i)
            {
                if (errors[i].ExpectedLevel != level)
                {
                    continue;
                }

                error = errors[i];
                break;
            }

            if (ReferenceEquals(error, null))
            {
                Debug.Log("could not find entry for level: " + level);
                return;
            }
            
            GameManager.OnMistakeMade(StateName, isTooEarly ? error.NotThereMessage : error.AlreadyCompletedMessage);
        }
    }
}