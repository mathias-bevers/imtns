using System.Collections.Generic;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class PopupManager : MonoBehaviour
    {
        private const string STATE_COMPLETED_MESSAGE = "Je hebt deze mini-game voltooid!";
        private const string CANNOT_GO_HERE_MESSAGE = "Je kan hier niet heen!";

        private static PopupManager _instance;
        public static PopupManager Instance
        {
            get
            {
                if (!ReferenceEquals(null, _instance))
                {
                    return _instance;
                }
                
                _instance = FindAnyObjectByType<PopupManager>();

                if (!ReferenceEquals(null, _instance))
                {
                    return _instance;
                }
                
                _instance = Instantiate(Resources.Load<PopupManager>("PopupManager"));
                _instance.gameObject.name = "RESOURCES_PopupManager";
                _instance.Initialize();
                return _instance;
            }
        }

        public Popup Popup { get; private set; } = null;
        private readonly Queue<PopupInfo> queue = new();
        private static bool _isInitialized = false;

        private void Awake()
        {
            if (ReferenceEquals(null, _instance))
            {
                _instance = this;
                Initialize();
            }
            else if (!ReferenceEquals(_instance, this))
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            Popup = GetComponentInChildren<Popup>(true);
            Popup.Initialize();

            StateMachine.Instance.transitionFailedEvent += OnStateTransitionFailed;

            Popup.closeEvent += _ => OnPopupClose();
            Popup.Close();
            
            DontDestroyOnLoad(gameObject);

            _isInitialized = true;
        }

        private void OnPopupClose()
        {
            if (queue.Count < 1)
            {
                return;
            }

            ShowPopup();
        }

        public void CreatePopup(string message, Popup.MessageType type, string title = null)
        {
            queue.Enqueue(new PopupInfo(message, type, title));

            if (Popup.gameObject.activeInHierarchy)
            {
                return;
            }

            ShowPopup();
        }

        private void ShowPopup()
        {
            PopupInfo info = queue.Dequeue();
            Popup.Show(info.Message, info.Type, info.Title);
        }

        private void OnStateTransitionFailed(string message)
        {
            CreatePopup(message, Popup.MessageType.Incorrect, CANNOT_GO_HERE_MESSAGE);
        }

        private readonly struct PopupInfo
        {
            public Popup.MessageType Type { get; }
            public string Message { get; }
            public string Title { get; }

            public PopupInfo(string message, Popup.MessageType type, string title = null)
            {
                Message = message;
                Type = type;
                Title = string.IsNullOrEmpty(title) ? type.ToString() : title;
            }
        }
    }
}