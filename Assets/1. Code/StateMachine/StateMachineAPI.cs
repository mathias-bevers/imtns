using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.StateMachine
{
    [CreateAssetMenu(fileName = "StateMachine API", menuName = "CleanRoom/StateMachine API",
        order = 0)]
    public class StateMachineAPI : ScriptableObject
    {
        [SerializeField, Scene] private int bootupScene;
        
        public void GoToNextRoom()
        {
            StateMachine.Instance.GoToNextRoom();
        }

        public void EnterState(State state)
        {
            StateMachine.Instance.EnterState(state);
        }

        public void Reset()
        {
            SaveSystem.DeleteAllSaves();
            DestroyImmediate(StateMachine.Instance.gameObject);
            DestroyImmediate(StateMachine.Instance);
            SceneManager.LoadScene(bootupScene);
        }
    }
}