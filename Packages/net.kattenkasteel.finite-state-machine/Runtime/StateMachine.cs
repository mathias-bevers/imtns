using UnityEngine;
using UnityEngine.SceneManagement;

namespace KattenKasteel.FSM
{
    public class StateMachine : Singleton<StateMachine>
    {
        [SerializeField] private State initialState;
        public State ActiveState { get; private set; }
        private State[] states;

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
            ActiveState = initialState;
            states = Resources.LoadAll<State>("States");

            MakeTransition(new Transition { Target = initialState });
        }

        public void MakeTransition(Transition transition)
        {
            if (!transition.CanTransition())
            {
                Debug.Log("Not all conditions are met to transition");
                return;
            }

            SceneManager.LoadScene(transition.Target.SceneIndex);
            ActiveState = transition.Target;
        }

        public void CompleteActiveState()
        {
            Debug.Log("Completed State: " + ActiveState.Name);
            ActiveState.IsCompleted = true;
        }
    }
}