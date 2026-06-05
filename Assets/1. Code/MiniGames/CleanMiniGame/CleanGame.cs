using KattenKasteel.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class CleanGame : Singleton<CleanGame>
    {
        [field: SerializeField] public DragAndSnap Wipe { get; private set; }
        [field: SerializeField] public Tablet Tablet { get; private set; }
        [field: SerializeField] public Button WipeBox { get; private set; }
        [field: SerializeField] public Button BagDispenser { get; private set; }
        [field: SerializeField] public Transitioner OnCompleteTransition { get; private set; }

        [SerializeField] private CleaningErrors[] errors;
        [SerializeField] private Transform toolContainer; 
        [SerializeField] private GameObject interactables;

        private GameManager gameManager;
        private string stateName;

        public void StartMiniGame()
        {
            stateName = StateMachine.Instance.ActiveState.StateName;
            gameManager = GameManager.Instance;
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
            
            gameManager.OnMistakeMade(stateName, isTooEarly ? error.NotThereMessage : error.AlreadyCompletedMessage);
        }
    }
}