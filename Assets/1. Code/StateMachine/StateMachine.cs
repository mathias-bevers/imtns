using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CleanRoom.StateMachine
{
    /// <summary>
    ///     This class keeps track of the <see cref="State" />s, update, and switches
    ///     them.
    /// </summary>
    public class StateMachine : Singleton<StateMachine>
    {
        private enum TransitionType { RoomToRoom, RoomToGame, GameToRoom }

        [SerializeField] private RoomState initialState;
        public State ActiveState { get; private set; } = null;

        private HashSet<string> completedStates = null;

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);

            base.Awake();

            completedStates = new HashSet<string>();
            ActiveState = initialState;
            SceneManager.LoadScene(ActiveState.SceneName, LoadSceneMode.Single);
        }

        /// <summary>
        ///     This method tries to enter the requested state. When it fails false will be
        ///     returned.
        /// </summary>
        /// <param name="state">The state needs to be entered</param>
        /// <returns>true is a new state is entered.</returns>
        private bool TryEnterState(State state)
        {
            if (state == ActiveState || !state.CanEnter)
            {
                return false;
            }

            if (completedStates.Contains(state.StateName))
            {
                return false;
            }

            TransitionType transitionType =
                DetermineTransitionType(ActiveState.GetType(), state.GetType());

            if (transitionType != TransitionType.RoomToGame && ActiveState is
                    { CanExit: false })
            {
                return false;
            }

            ActiveState.completedEvent -= OnStateCompleted;

            switch (transitionType)
            {
                case TransitionType.RoomToRoom:
                    ActiveState?.Exit();
                    SceneLoader.LoadScene(state.SceneName, LoadSceneMode.Single,
                        OnSceneLoaded);
                    break;
                case TransitionType.RoomToGame:
                    SceneLoader.LoadScene(state.SceneName, LoadSceneMode.Additive,
                        OnSceneLoaded);
                    break;
                case TransitionType.GameToRoom:
                    ActiveState?.Exit();
                    SceneManager.UnloadSceneAsync(ActiveState?.SceneName);
                    OnSceneLoaded(state.SceneName);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

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
            if (ActiveState is not RoomState activeRoomState)
            {
                Debug.LogWarning("can only move to next room from a room");
                return;
            }

            EnterState(activeRoomState.NextRoom);
        }

        public bool IsStateCompleted(string stateName) =>
            completedStates.Contains(stateName);

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
            else if (ot == typeof(RoomState) &&
                     (tt == typeof(GameState) || tt.IsSubclassOf(typeof(GameState))))
            {
                type = TransitionType.RoomToGame;
            }
            else if ((ot == typeof(GameState) || ot.IsSubclassOf(typeof(GameState))) &&
                     tt == typeof(RoomState))
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

        private void OnSceneLoaded(string sceneName)
        {
            ActiveState = FindObjectsByType<State>(FindObjectsSortMode.InstanceID)
                .FirstOrDefault(s => string.Equals(s.SceneName, sceneName));

            if (ReferenceEquals(null, ActiveState))
            {
                throw new NullReferenceException($"could not find state for: {sceneName}");
            }
            
            ActiveState.completedEvent += OnStateCompleted;
            ActiveState.Enter();
        }

        private void OnStateCompleted(string stateName)
        {
            completedStates.Add(stateName);
            Debug.Log("completed state: " + stateName);
        }
    }
}