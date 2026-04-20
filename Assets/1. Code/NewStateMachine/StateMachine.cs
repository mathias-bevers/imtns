using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.NewStateMachine
{
    /// <summary>
    ///     This class keeps track of the <see cref="State" />s, update, and switches
    ///     them.
    /// </summary>
    public class StateMachine : Singleton<StateMachine>
    {
        private enum TransitionType { RoomToRoom, RoomToGame, GameToRoom }

        [SerializeField] private RoomState initialState;
        private State activeState = null;

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);

            base.Awake();

            activeState = initialState;
            SceneManager.LoadScene(activeState.SceneName, LoadSceneMode.Single);
        }

        /// <summary>
        ///     This method tries to enter the requested state. When it fails false will be
        ///     returned.
        /// </summary>
        /// <param name="state">The state needs to be entered</param>
        /// <returns>true is a new state is entered.</returns>
        private bool TryEnterState(State state)
        {
            if (state == activeState || !state.CanEnter)
            {
                return false;
            }

            TransitionType transitionType =
                DetermineTransitionType(activeState.GetType(), state.GetType());

            if (transitionType != TransitionType.RoomToGame && activeState is
                    { CanExit: false })
            {
                return false;
            }

            switch (transitionType)
            {
                case TransitionType.RoomToRoom:
                    activeState?.Exit();
                    SceneManager.LoadScene(state.SceneName, LoadSceneMode.Single);
                    state.Enter();
                    break;
                case TransitionType.RoomToGame:
                    SceneManager.LoadScene(state.SceneName, LoadSceneMode.Additive);
                    ((RoomState)activeState).UnfocusEvent?.Invoke();
                    state.Enter();
                    break;
                case TransitionType.GameToRoom:
                    SceneManager.UnloadSceneAsync(state.SceneName);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            activeState = state;
            return true;
        }

        /// <summary>
        ///     Wrapper for entering a state
        /// </summary>
        /// <param name="state">State to be entered</param>
        public void EnterState(State state)
        {
            bool result = TryEnterState(state);

            if (result)
            {
                return;
            }

            Debug.LogWarning($"could not enter state: {state.name}");
        }


        public void GoToNextRoom()
        {
            if (activeState is not RoomState activeRoomState)
            {
                Debug.LogWarning("can only move to next room from a room");
                return;
            }

            EnterState(activeRoomState.NextRoom);
        }

        /// <summary>
        ///     Determines the type of the state transition.
        /// </summary>
        /// <param name="ot">The type of the state that is left</param>
        /// <param name="tt">The type of the state that is entered</param>
        /// <returns>A enum based on the type</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when a game to game transition happens
        /// </exception>
        private static TransitionType DetermineTransitionType(Type ot, Type tt)
        {
            TransitionType type;

            if (ot == typeof(RoomState) && tt == typeof(RoomState))
            {
                type = TransitionType.RoomToRoom;
            }
            else if (ot == typeof(RoomState) && tt == typeof(GameState))
            {
                type = TransitionType.RoomToGame;
            }
            else if (ot == typeof(GameState) && tt == typeof(RoomState))
            {
                type = TransitionType.GameToRoom;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Invalid transition: {ot.Name}->{tt.Name}");
            }

            return type;
        }
    }
}