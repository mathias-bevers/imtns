using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    public abstract class GameState : MonoBehaviour
    {
        public bool IsActive { get; private set; } = false;
        
        private readonly HashSet<IGameStateObject> gameStateObjects = new();
        private Transform cachedTransform;

        public virtual void OnEnter()
        {
            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(true);
            }

            IsActive = true;
        }

        public void AddStateObject(IGameStateObject gameStateObject) => gameStateObjects.Add(gameStateObject);

        public void RemoveStateObject(IGameStateObject gameStateObject) => gameStateObjects.Remove(gameStateObject);

        public virtual void Tick(float deltaTime)
        {
            foreach (IGameStateObject stateObject in gameStateObjects)
            {
                stateObject.Tick(deltaTime);
            }
        }

        public virtual void FixedTick(float fixedDeltaTime)
        {
            foreach (IGameStateObject stateObject in gameStateObjects)
            {
                stateObject.FixedTick(fixedDeltaTime);
            }
        }

        public virtual void OnExit()
        {
            IsActive = false;

            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}