using CleanRoom.Menus;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.Interactables
{
    public class InteractionZone : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";

        [field: SerializeField] public UnityEvent OnInteract { get; private set; }
        private OnScreenControllerMenu hud;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(PLAYER_TAG))
            {
                 return;  
            }

            hud ??= MenuManager.Instance.GetMenuOfType<OnScreenControllerMenu>();
            hud.InteractionButton.OnInteractionZoneEnter(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(PLAYER_TAG))
            {
                return;
            }
            
            hud ??= MenuManager.Instance.GetMenuOfType<OnScreenControllerMenu>();
            hud.InteractionButton.OnInteractionZoneExit(this);
        }
    }
}