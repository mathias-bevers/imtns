using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public abstract class GameState : MonoBehaviour
    {
        public bool IsActive { get; private set; } = false;
        
        private readonly List<IGameStateObject> gameStateObjects = new();
        private Transform cachedTransform;

        public virtual void Initialize() { }

        protected virtual void OnEnter() { }

        public void Enter(bool skipOnEnter = false)
        {
            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(true);
            }

            IsActive = true;

            if (skipOnEnter)
            {
                return;
            }
            
            OnEnter();
        }

        public void AddStateObject(IGameStateObject gameStateObject)
        {
            if (gameStateObjects.Contains(gameStateObject))
            {
                return;
            }
            
            gameStateObjects.Add(gameStateObject);
        }

        public void RemoveStateObject(IGameStateObject gameStateObject) => gameStateObjects.Remove(gameStateObject);

        public virtual void Tick(float deltaTime)
        {
            for (int i = gameStateObjects.Count - 1; i >= 0; --i)
            {
                if (ReferenceEquals(null, gameStateObjects[i]))
                {
                    continue;
                }
                
                gameStateObjects[i].Tick(deltaTime);
            }
        }

        public virtual void FixedTick(float fixedDeltaTime)
        {
            for (int i = gameStateObjects.Count - 1; i >= 0; --i)
            {
                if (ReferenceEquals(null, gameStateObjects[i]))
                {
                    continue;
                }
                
                gameStateObjects[i].FixedTick(fixedDeltaTime);
            }
        }
        
        protected virtual void OnExit() { }

        public void Exit(bool skipOnExit = false)
        {
            IsActive = false;

            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(false);
            }

            if (skipOnExit)
            {
                return;
            }
            
            OnExit();
        }
    }
}