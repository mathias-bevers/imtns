using UnityEngine;
using UnityEngine.EventSystems;

namespace CleanRoom.UserInterface
{
    public class DisableOnClick : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }
    }
}