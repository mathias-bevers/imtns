using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class Popup : MonoBehaviour
    {
        public enum MessageType { Incorrect, Correct, Feedback }

        private static readonly Regex NUMBERS_REGEX = new(@"\d+");
        private static readonly Dictionary<MessageType, Color32> COLOR_MAP = new()
        {
            { MessageType.Feedback, new Color32(162, 242, 206, 255) },
            { MessageType.Correct, new Color32(166, 209, 137, 255) },
            { MessageType.Incorrect, new Color32(231, 130, 132, 255) }
        };

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Transform starsParent;
        private MessageType? showingMessageType = null;

        private void Awake()
        {
            title.overrideColorTags = true;
            title.color = COLOR_MAP[MessageType.Correct];
        }

        public event Action closeEvent;

        public void Initialize(string text, MessageType messageType, string title)
        {
            if (messageType == MessageType.Feedback)
            {
                Match match = NUMBERS_REGEX.Match(text);
                int stars = int.Parse(match.Value);

                text = text[match.Value.Length..];
                
                SetStars(stars);
            }

            showingMessageType = messageType;
            this.title.SetText(title);
            this.title.color = COLOR_MAP[messageType];
            this.text.SetText(text);
            
            gameObject.SetActive(true);
        }

        public void Close()
        {
            starsParent.gameObject.SetActive(false);
            gameObject.SetActive(false);
            
            if (showingMessageType == MessageType.Feedback)
            {
                StateMachine.StateMachine.Instance.GoToNextRoom();
            }

            showingMessageType = null;
            
            closeEvent?.Invoke();
        }

        public void SetStars(int count)
        {
            starsParent.gameObject.SetActive(true);
            for (int i = 0; i < starsParent.childCount; ++i)
            {
                starsParent.GetChild(i).gameObject.SetActive(i < count);
            }
        }
    }
}