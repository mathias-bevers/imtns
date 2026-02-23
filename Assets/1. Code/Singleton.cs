using UnityEngine;

namespace CleanRoom
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static bool IsInitialized => _instance != null;

        public static T Instance => GetInstance();

        public virtual void Awake()
        {
            if (ReferenceEquals(null, _instance))
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        private static T GetInstance()
        {
            if (!ReferenceEquals(null, _instance))
            {
                return _instance;
            }

            _instance = FindFirstObjectByType<T>();
            
            if (!ReferenceEquals(null, _instance))
            {
                return _instance;
            }

            GameObject gameObject = new();
            gameObject.name = typeof(T).Name;
            _instance = gameObject.AddComponent<T>();
            return _instance;
        }
    }
}