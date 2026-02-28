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
        private static readonly Dictionary<Level, Color> LEVEL_COLORS = new()
        {
            { Level.Info, new Color(198, 208, 245) },
            { Level.Warning, new Color(231, 130, 132) },
            { Level.Error, new Color(231, 130, 132) }
        };

        public void Initialize(string message, Level level)
        {
            text.SetText(message);
            text.color = LEVEL_COLORS[level];

            Destroy(gameObject, displayTime);
        }

        public enum Level { Info, Warning, Error }

        public void OnPointerClick(PointerEventData eventData)
        {
            Destroy(gameObject);
        }
    }
}