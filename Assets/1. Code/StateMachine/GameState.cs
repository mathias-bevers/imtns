using UnityEngine;

namespace CleanRoom.StateMachine
{
    public abstract class GameState : MonoBehaviour
    {
        private Transform cachedTransform;

        public virtual void OnEnter()
        {
            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public abstract void Tick();

        public abstract void FixedTick();

        public virtual void OnExit()
        {
            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}