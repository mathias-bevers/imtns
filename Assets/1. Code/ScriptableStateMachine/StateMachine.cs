using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.ScriptableStateMachine
{
    /// <summary>
    ///     This class keeps track of the <see cref="ScriptableState" />s, update, and switches
    ///     them.
    /// </summary>
    public class StateMachine : Singleton<StateMachine>
    {
        private enum TransitionType { RoomToRoom, RoomToGame, GameToRoom }

        [SerializeField] private ScriptableRoomState initialState;
        private ScriptableState activeState = null;

        public override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            SceneManager.LoadScene(initialState.SceneName, LoadSceneMode.Single);
            activeState = initialState;
        }

        /// <summary>
        ///     This method tries to enter the requested state. When it fails false will be
        ///     returned.
        /// </summary>
        /// <param name="state">The state needs to be entered</param>
        /// <returns>true is a new state is entered.</returns>
        private bool TryEnterState(ScriptableState state)
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
                    activeState.Exit();
                    SceneManager.LoadScene(state.SceneName, LoadSceneMode.Single);
                    state.Enter();
                    break;
                case TransitionType.RoomToGame:
                    SceneManager.LoadScene(state.SceneName, LoadSceneMode.Additive);
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

        public void EnterState(ScriptableState state)
        {
            bool result = TryEnterState(state);

            if (result)
            {
                return;
            }

            Debug.LogWarning($"could not enter state: {state.name}");
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

            if (ot == typeof(ScriptableRoomState) && tt == typeof(ScriptableRoomState))
            {
                type = TransitionType.RoomToRoom;
            }
            else if (ot == typeof(ScriptableRoomState) && tt == typeof(ScriptableGameState))
            {
                type = TransitionType.RoomToGame;
            }
            else if (ot == typeof(ScriptableGameState) && tt == typeof(ScriptableRoomState))
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