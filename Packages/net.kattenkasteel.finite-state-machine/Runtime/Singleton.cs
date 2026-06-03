using UnityEngine;

namespace KattenKasteel
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static bool IsInitialized => _instance != null;

        public static T Instance => GetInstance();

        private static bool _isPendingDestroy = false;

        public virtual void Awake()
        {
            if (ReferenceEquals(null, _instance))
            {
                _instance = this as T;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            _isPendingDestroy = true;
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

            if (_isPendingDestroy)
            {
                return null;
            }

            GameObject gameObject = new();
            gameObject.name = typeof(T).Name;
            _instance = gameObject.AddComponent<T>();
            return _instance;
        }
    }
}
