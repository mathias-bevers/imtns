using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class ConditionalOnClick : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public bool PreventInvoke { get; set; } = false;
        [field: SerializeField] public UnityEvent OnClick { get; private set; }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (PreventInvoke)
            {
                return;
            }

            OnClick.Invoke();
        }
    }
}