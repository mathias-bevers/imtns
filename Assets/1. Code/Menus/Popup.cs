using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CleanRoom.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Menus
{
    public class Popup : MonoBehaviour
    {
        public enum MessageType { Incorrect, Correct, StartMiniGame, Feedback, CompletedMiniGame }

        private static readonly Regex NUMBERS_REGEX = new(@"\d+");
        private static readonly Dictionary<MessageType, Color32> COLOR_MAP = new()
        {
            { MessageType.Feedback, new Color32(162, 242, 206, 255) },
            { MessageType.StartMiniGame, new Color32(162, 242, 206, 255) },
            { MessageType.Correct, new Color32(166, 209, 137, 255) },
            { MessageType.CompletedMiniGame, new Color32(166, 209, 137, 255) },
            { MessageType.Incorrect, new Color32(231, 130, 132, 255) }
        };

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject starsParent;
        [SerializeField] private GameObject backgroundPanel;
        
        private MessageType? showingMessageType = null;
        private Button[] stars;
        private bool isInitialized;

        public event Action<MessageType> closeEvent;

        public void Initialize()
        {
            if (isInitialized)
            {
                return;
            }
            
            title.overrideColorTags = true;
            stars = starsParent.GetComponentsInChildren<Button>(true);
            Array.Sort(stars, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

            isInitialized = true;
        }

        public void Show(string text, MessageType messageType, string title)
        {
            if (!isInitialized)
            {
                throw new NotSupportedException("cannot show popup without being initialized");
            }
            
            if (messageType >= MessageType.Feedback)
            {
                Match match = NUMBERS_REGEX.Match(text);
                int starCount = int.Parse(match.Value);

                text = text[match.Value.Length..];
                
                SetStars(starCount);
            }

            showingMessageType = messageType;
            this.title.SetText(title);
            this.title.color = COLOR_MAP[messageType];
            this.text.SetText(text);
            
            backgroundPanel.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            starsParent.gameObject.SetActive(false);
            gameObject.SetActive(false);
            backgroundPanel.SetActive(false);

            MessageType? shownMessageType = showingMessageType;
            showingMessageType = null;
            if (!ReferenceEquals(null, shownMessageType))
            {
                SoundManager.Instance.PlaySFX("click");
                closeEvent?.Invoke(shownMessageType.Value);
            }
        }

        public void SetStars(int count)
        {
            if (stars.IsNullOrEmpty())
            {
                throw new Exception("stars array is null or empty");
            }
            
            starsParent.gameObject.SetActive(true);
            for (int i = 0; i < stars.Length; ++i)
            {
                stars[i].interactable = i < count;
            }
        }
    }
}