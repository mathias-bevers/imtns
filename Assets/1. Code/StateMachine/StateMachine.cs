using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class StateMachine : Singleton<StateMachine>
    {
        [SerializeField] private RoomState startingRoomState;
        private readonly Dictionary<Type, State> states = new();
        private State activeGameState = null;

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

        private void Update()
        {
            activeGameState.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            activeGameState.FixedTick(Time.fixedDeltaTime);
        }

        public void SwitchToState<T>() where T : State => SwitchToState(states[typeof(T)]);

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

        public T GetGameState<T>() where T : GameState =>
            (T)states[typeof(T)];
    }
}