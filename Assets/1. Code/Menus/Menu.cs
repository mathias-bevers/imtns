using System;
using UnityEngine;

namespace CleanRoom.Menus
{
    public abstract class Menu : MonoBehaviour
    {
        [field: SerializeField] public bool IsHUD { get; protected set; }

        public bool IsOpen { get; private set; }
        public bool CanBeOpened { get; protected set; } = true;
        public bool CanBeClosed { get; protected set; } = true;

        public event Action openedEvent;
        public event Action closedEvent;

        private GameObject content;

        protected GameObject Content
        {
            get
            {
                if (!ReferenceEquals(null, content))
                {
                    return content;
                }

                if (transform.childCount != 1)
                {
                    throw new NotSupportedException(
                        $"The only child of {GetType().Name} should be the \"Content\" GameObject.");
                }

                content = transform.GetChild(0).gameObject;
                return content;
            }
        }

        protected virtual void Start()
        {
            if (IsHUD)
            {
                Open();
            }
            else
            {
                Close();
            }

            MenuManager.Instance.RegisterMenu(this);
        }

        public void Open()
        {
            if (!CanBeOpened)
            {
                Debug.LogWarning($"{GetType().Name} does not meet the opening criteria");
                return;
            }

            Content.SetActive(true);
            IsOpen = true;
            openedEvent?.Invoke();
        }

        public void Close()
        {
            if (!CanBeClosed)
            {
                Debug.LogWarning($"{GetType().Name} does not meet the closing criteria");
                return;
            }

            Content.SetActive(false);
            IsOpen = false;
            closedEvent?.Invoke();
        }

        private void OnDestroy()
        {
            if (!MenuManager.IsInitialized)
            {
                return;
            }

            MenuManager.Instance.UnregisterMenu(this);
        }
    }
}