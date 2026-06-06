using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KattenKasteel.FSM
{
    public class StateMachine : MonoBehaviour
    {
        private static StateMachine _instance;
        public static StateMachine Instance
        {
            get
            {
                if (!ReferenceEquals(null, _instance))
                {
                    return _instance;
                }

                _instance = FindFirstObjectByType<StateMachine>();

                if (!ReferenceEquals(null, _instance))
                {
                    return _instance;
                }

                GameObject gameObject = new();
                gameObject.name = "STATE_MACHINE";
                _instance = gameObject.AddComponent<StateMachine>();
                return _instance;
            }
        }

        [SerializeField] private State initialState;
        
        public State ActiveState { get; private set; }

        public event Action<State> stateCompletedEvent;
        public event Action<string> transitionFailedEvent;
        private State[] states;

        public void Awake()
        {
            if (!ReferenceEquals(null, _instance))
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            states = Resources.LoadAll<State>("States");

            if (ReferenceEquals(null, initialState))
            {
                ActiveState = GetState(state => state.SceneIndex == SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                MakeTransition(new Transition { Target = initialState });
            }
        }

        public void MakeTransition(Transition transition)
        {
            if (!transition.CanTransition(out string conditionMessages))
            {
                transitionFailedEvent?.Invoke(conditionMessages);
                Debug.Log("Not all conditions are met to transition:\n" + conditionMessages);
                return;
            }
            
            SceneManager.LoadScene(transition.Target.SceneIndex);

            if (!ReferenceEquals(null, ActiveState))
            {
                ActiveState.HasBeenVisited = true;
            }
            
            ActiveState = transition.Target;
        }

        public void CompleteActiveState()
        {
            Debug.Log("Completed State: " + ActiveState.StateName);
            stateCompletedEvent?.Invoke(ActiveState);
            ActiveState.IsCompleted = true;
        }

        public State GetState(Func<State, bool> predicate)
        {
            if (ReferenceEquals(null, states) || states.Length == 0)
            {
                states = Resources.LoadAll<State>("States");
            }

            for (int i = 0; i < states.Length; ++i)
            {
                if (!predicate(states[i]))
                {
                    continue;
                }

                return states[i];
            }

            throw new NullReferenceException("could not find state with the current conditions");
        }

        public static void ResetStates()
        {
            foreach (State state in Resources.LoadAll<State>("States"))
            {
                state.Reset();
            }
        }
    }
}
