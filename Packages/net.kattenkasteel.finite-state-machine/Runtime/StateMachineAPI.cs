using UnityEngine;

namespace KattenKasteel.FSM
{
    [CreateAssetMenu(fileName = "StateMachine API", menuName = "KattenKasteel/FSM/API")]
    public class StateMachineAPI : ScriptableObject
    {
        public void MakeTransition(Transition transition)
        {
            if (ReferenceEquals(null, transition))
            {
                Debug.LogError("Make sure to add the transition");
                return;
            }
            
            StateMachine.Instance.MakeTransition(transition);
        }
        
        public void CompleteActiveState()
        {
            StateMachine.Instance.CompleteActiveState();
        }

        public void ResetStates()
        {
            StateMachine.ResetStates();
        }
    }
}