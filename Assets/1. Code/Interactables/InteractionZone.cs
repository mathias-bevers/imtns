using CleanRoom.Menus;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.Interactables
{
    public class InteractionZone : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent OnInteract { get; private set; }
        private static OnScreenControllerMenu _hud;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();

            if (ReferenceEquals(null, player))
            {
                return;
            }

            _hud ??= MenuManager.Instance.GetMenuOfType<OnScreenControllerMenu>();
            _hud.InteractionButton.OnInteractionZoneEnter(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();

            if (ReferenceEquals(null, player))
            {
                return;
            }

            _hud ??= MenuManager.Instance.GetMenuOfType<OnScreenControllerMenu>();
            _hud.InteractionButton.OnInteractionZoneExit(this);
        }
    }
}