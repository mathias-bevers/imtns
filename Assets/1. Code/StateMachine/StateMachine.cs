using System;
using System.Collections.Generic;
using System.Linq;
using CleanRoom.Menus;
using Newtonsoft.Json.Linq;
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
        [SerializeField] private RoomState initialState;
        public State ActiveState { get; private set; } = null;
        private HashSet<string> completedStates = null;

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);

            base.Awake();

            completedStates = new HashSet<string>(GetCompletedStates());
            SceneLoader.LoadScene(initialState.SceneName, LoadSceneMode.Single, OnSceneLoaded);
        }

        private IEnumerable<string> GetCompletedStates() =>
            from KeyValuePair<string, JToken> kvp in SaveSystem.LoadGameStates()
            where kvp.Value["is_completed"]!.ToObject<bool>()
            select kvp.Key;


        /// <summary>
        ///     This method tries to enter the requested state. When it fails false will be
        ///     returned.
        /// </summary>
        /// <param name="state">The state needs to be entered</param>
        /// <param name="errorMessage">If the switch fails, the error message will tell why</param>
        /// <returns>true is a new state is entered.</returns>
        private bool TryEnterState(State state, out string errorMessage)
        {
            if (state == ActiveState || !state.CanEnter)
            {
                errorMessage = "could not enter new state";
                return false;
            }

            if (completedStates.Contains(state.GetType().Name))
            {
                errorMessage = "new state is already completed";
                return false;
            }

            TransitionType transitionType = DetermineTransitionType(ActiveState.GetType(), state.GetType());

            if (transitionType != TransitionType.RoomToGame && ActiveState is { CanExit: false })
            {
                errorMessage = "could not exit current state";
                return false;
            }

            ActiveState.completedEvent -= OnStateCompleted;
            ActiveState.completedEvent -= OnGameStateCompleted;

            switch (transitionType)
            {
                case TransitionType.RoomToRoom:
                    ActiveState?.Exit();
                    SceneLoader.LoadScene(state.SceneName, LoadSceneMode.Single, OnSceneLoaded);
                    break;
                case TransitionType.RoomToGame:
                    SceneLoader.LoadScene(state.SceneName, LoadSceneMode.Additive, OnSceneLoaded);
                    break;
                case TransitionType.GameToRoom:
                    ActiveState?.Exit();
                    SceneManager.UnloadSceneAsync(ActiveState?.SceneName);
                    OnSceneLoaded(state.SceneName);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        ///     Wrapper for entering a state
        /// </summary>
        /// <param name="state">State to be entered</param>
        public void EnterState(State state)
        {
            if (TryEnterState(state, out string message))
            {
                return;
            }

            Debug.Log(message);
        }

        public void GoToNextRoom()
        {
            if (ActiveState is RoomState activeRoomState)
            {
                if (ReferenceEquals(null, activeRoomState.NextRoom))
                {
                    Debug.LogWarning($"the room: {activeRoomState.StateName} has no next room");
                    return;
                }

                EnterState(activeRoomState.NextRoom);
                return;
            }

            if (ActiveState is GameState activeGameState)
            {
                if (ReferenceEquals(null, activeGameState.ParentState))
                {
                    Debug.LogError($"the game: {activeGameState.StateName} has no parent state");
                    return;
                }

                EnterState(activeGameState.ParentState);
                return;
            }

            throw new NotSupportedException($"Could not process next room for type: {ActiveState.GetType()}");
        }

        public bool IsStateCompleted(string stateName) =>
            completedStates.Contains(stateName);

        /// <summary>
        ///     Determines the type of the state transition.
        /// </summary>
        /// <param name="ot">The type of the state that is left</param>
        /// <param name="tt">The type of the state that is entered</param>
        /// <returns>An enum based on the type</returns>
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
            else if (ot == typeof(RoomState) && (tt == typeof(GameState) || tt.IsSubclassOf(typeof(GameState))))
            {
                type = TransitionType.RoomToGame;
            }
            else if ((ot == typeof(GameState) || ot.IsSubclassOf(typeof(GameState))) && tt == typeof(RoomState))
            {
                type = TransitionType.GameToRoom;
            }
            else
            {
                throw new InvalidOperationException($"Invalid transition: {ot.Name}->{tt.Name}");
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
            if (ActiveState is GameState)
            {
                ActiveState.completedEvent += OnGameStateCompleted;
            }

            ActiveState.Enter();
        }

        private void OnStateCompleted(string stateName)
        {
            completedStates.Add(stateName);
        }

        private void OnGameStateCompleted(string stateTypeName)
        {
            JToken token = SaveSystem.LoadGameStates()[stateTypeName];
            if (ReferenceEquals(null, token))
            {
                throw new NullReferenceException("could not find token for state: " + stateTypeName);
            }
            
            if (token["feedback"] is not JArray array)
            {
                throw new InvalidCastException("get an j-array for the key \"feedback\"");
            }
            
            GameState gameState = ActiveState as GameState;
            int stars = Mathf.Max(3 - array.Count, 0);

            MenuManager.Instance.GetMenuOfType<PopupMenu>().CreatePopup(stars + gameState?.CompletionMessage,
                Popup.MessageType.Feedback, stateTypeName + " is voltooid!");
        }

        private enum TransitionType { RoomToRoom, RoomToGame, GameToRoom }
    }
}