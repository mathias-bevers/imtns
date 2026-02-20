using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class GameStateController : Singleton<GameStateController>
    {
        [SerializeField] private GameState startingGameState;
        private readonly Dictionary<Type, GameState> gameStates = new();
        private GameState activeGameState = null;

        public override void Awake()
        {
            base.Awake();

            GameState[] foundStates =
                FindObjectsByType<GameState>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < foundStates.Length; ++i)
            {
                GameState gameState = foundStates[i];
                gameStates.Add(gameState.GetType(), gameState);
                gameState.OnExit();
            }

            SwitchToState(startingGameState);
        }

        private void Update()
        {
            activeGameState.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            activeGameState.FixedTick(Time.fixedDeltaTime);
        }

        public void SwitchToState<T>() => SwitchToState(gameStates[typeof(T)]);

        public void SwitchToState<T>(T state) where T : GameState
        {
            if (ReferenceEquals(state, activeGameState))
            {
                Debug.LogWarning("trying to set same type, ignoring");
                return;
            }

            if (!ReferenceEquals(null, activeGameState))
            {
                activeGameState.OnExit();
            }

            state.OnEnter();

            activeGameState = state;
        }
    }
}