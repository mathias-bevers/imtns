using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class Popup : MonoBehaviour
    {
        public enum MessageType
        {
            Incorrect,
            Correct,
            Feedback
        }

        private static readonly Dictionary<MessageType, Color32> COLOR_MAP = new()
        {
            { MessageType.Correct, new Color32(166, 209, 137, 255) },
            { MessageType.Incorrect, new Color32(231, 130, 132, 255) }
        };
        
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI text;
        public event Action closeEvent;

        private void Awake()
        {
            title.overrideColorTags = true;
            title.color = COLOR_MAP[MessageType.Correct];
        }

        public void Initialize(string text, MessageType messageType, string title)
        {
            if (messageType == MessageType.Feedback)
            {
                //TODO: init feedback message
                return;
            }
             
            this.title.SetText(title);
            this.title.color = COLOR_MAP[messageType];
            this.text.SetText(text);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            closeEvent?.Invoke();
        }
    }
}