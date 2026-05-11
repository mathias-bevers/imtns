using UnityEngine;

namespace CleanRoom.StateMachine
{
    [CreateAssetMenu(fileName = "StateMachine API", menuName = "CleanRoom/StateMachine API",
        order = 0)]
    public class StateMachineAPI : ScriptableObject
    {
        public void GoToNextRoom()
        {
            StateMachine.Instance.GoToNextRoom();
        }

        public void EnterState(State state)
        {
            StateMachine.Instance.EnterState(state);
        }

        public void ClearSaves()
        {
            SaveSystem.DeleteAllSaves();
        }
    }
}