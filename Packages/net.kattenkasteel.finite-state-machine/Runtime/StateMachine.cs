using System;
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

            LoadSceneMode loadMode = transition.Target.IsParent ? LoadSceneMode.Single : LoadSceneMode.Additive;
            SceneManager.LoadScene(transition.Target.SceneIndex, loadMode);
            ActiveState = transition.Target;
        }

        public void CompleteActiveState()
        {
            Debug.Log("Completed State: " + ActiveState.StateName);
            ActiveState.IsCompleted = true;
        }

        public State GetState(Func<State, bool> test)
        {
            if (ReferenceEquals(null, states) || states.Length == 0)
            {
                states = Resources.LoadAll<State>("States");
            }
            
            for (int i = 0; i < states.Length; ++i)
            {
                if (!test(states[i]))
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
                state.IsCompleted = false;
            }
        }
    }
}