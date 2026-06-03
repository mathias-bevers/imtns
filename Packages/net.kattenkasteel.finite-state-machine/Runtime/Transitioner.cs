using UnityEngine;

namespace KattenKasteel.FSM
{
    public class Transitioner : MonoBehaviour
    {
        [SerializeField] private Transition transition;

        public void Transition()
        {
            if (ReferenceEquals(null, transition))
            {
                Debug.LogError("the transition is null!");
                return;
            }
            
            if (ReferenceEquals(null, transition.Target))
            {
                Debug.LogError("the target state is null!");
                return;
            }
            
            StateMachine.Instance.MakeTransition(transition);
        }
    }
}