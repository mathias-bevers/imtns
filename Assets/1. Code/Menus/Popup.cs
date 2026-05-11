using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.Menus
{
    public class Popup : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float displayTime = 5f;

        public event Action<string> destroyEvent;
        
        private static readonly Dictionary<Level, Color32> LEVEL_COLORS = new()
        {
            { Level.Info, new Color32(198, 208, 245, 255) },
            { Level.Warning, new Color32(229, 200, 144, 255) },
            { Level.Error, new Color32(231, 130, 132, 255) }
        };

        public void Initialize(string message, Level level)
        {
            text.SetText(message);
            text.overrideColorTags = true;
            text.color = LEVEL_COLORS[level];

            Destroy(gameObject, displayTime);
        }

        public enum Level { Info, Warning, Error }

        public void OnPointerClick(PointerEventData eventData)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            destroyEvent?.Invoke(text.text);
        }
    }
}