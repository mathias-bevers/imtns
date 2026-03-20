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
                gameState.gameObject.SetActive(true);
                gameState.Initialize();
                gameState.Exit(true);
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

        public void SwitchToState<T>() where T : GameState => SwitchToState(gameStates[typeof(T)]);

        public void SwitchToState(GameState state)
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
            (T)gameStates[typeof(T)];
    }
}