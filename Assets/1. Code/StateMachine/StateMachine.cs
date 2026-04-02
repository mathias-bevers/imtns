using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    /// <summary>
    ///     This class keeps track of the <see cref="State" />s, update, and switches them.
    /// </summary>
    public class StateMachine : Singleton<StateMachine>
    {
        /// <summary>
        ///     The state the game is entered into first
        /// </summary>
        [SerializeField] private RoomState startingRoomState;

        /// <summary>
        ///     A map for converting types to their instances.
        /// </summary>
        private readonly Dictionary<Type, State> states = new();

        /// <summary>
        ///     The state that is currently active.
        /// </summary>
        private State activeGameState = null;

        /// <summary>
        ///     This method is called by unity, it searches for all <see cref="State" />s in the current scene,
        ///     it initializes and deactivates them. The <see cref="State.OnExit" /> method is skipped.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            GameState[] foundStates =
                FindObjectsByType<GameState>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < foundStates.Length; ++i)
            {
                GameState gameState = foundStates[i];
                states.Add(gameState.GetType(), gameState);
                gameState.gameObject.SetActive(true);
                gameState.Initialize();
                gameState.Exit(true);
            }

            SwitchToState(startingRoomState);
        }

        /// <summary>
        ///     Calls the <see cref="State.Tick" /> method from the <see cref="activeGameState" />.
        /// </summary>
        private void Update()
        {
            activeGameState.Tick(Time.deltaTime);
        }

        /// <summary>
        ///     Calls the <see cref="State.FixedTick" /> method from the <see cref="activeGameState" />.
        /// </summary>
        private void FixedUpdate()
        {
            activeGameState.FixedTick(Time.fixedDeltaTime);
        }

        /// <summary>
        ///     Switches to a state based on the type. It uses the <see cref="states" /> map to get the right instance.
        /// </summary>
        /// <typeparam name="T">The Type that needs to be switched to</typeparam>
        public void SwitchToState<T>() where T : State => SwitchToState(states[typeof(T)]);

        /// <summary>
        ///     This method sets the <paramref name="state" /> instance to active. It first calls the
        ///     <see
        ///         cref="State.Exit" />
        ///     on the currently active state, then calls the <see cref="State.Enter" /> state for
        ///     the new active state and sets the <see cref="activeGameState" />.
        /// </summary>
        /// <param name="state">The instance to switch to.</param>
        public void SwitchToState(State state)
        {
            if (ReferenceEquals(state, activeGameState))
            {
                Debug.LogWarning("trying to set same type, ignoring");
                return;
            }

            activeGameState?.Exit();
            state.Enter();

            activeGameState = state;
        }

        /// <summary>
        ///     This method gets the <see cref="State" /> instance form the <see cref="states" /> map.
        /// </summary>
        /// <typeparam name="T"><see cref="State" /> type to get.</typeparam>
        /// <returns>The instance of Type <typeparamref name="T" /></returns>
        public T GetGameState<T>() where T : GameState =>
            (T)states[typeof(T)];
    }
}