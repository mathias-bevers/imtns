using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public class GameStateController : Singleton<GameStateController>
    {
        [SerializeField] private GameState startingGameState;
        private Dictionary<Type, GameState> gameStates = new();
        private GameState activeGameState = null;

        public override void Awake()
        {
            base.Awake();

            GameState[] foundStates = FindObjectsByType<GameState>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < foundStates.Length; ++i)
            {
                GameState gameState = foundStates[i];
                gameStates.Add(gameState.GetType(), gameState);
            }
        }

        public void SwitchToState<T>() where T : GameState
        {
            SwitchToState(gameStates[typeof(T)]);
        }

        private void SwitchToState<T>(T state) where T : GameState
        {
            if (ReferenceEquals(state, activeGameState))
            {
                Debug.LogWarning("trying to set same type, ignoring");
                return;
            }
            
            activeGameState.OnExit();
            state.OnEnter();
            
            activeGameState = state;
        }

        private void Update()
        {
            activeGameState.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            activeGameState.FixedTick(Time.fixedDeltaTime);
        }
    }
}