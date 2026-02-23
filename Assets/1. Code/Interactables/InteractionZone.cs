using System;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.Interactables
{
    public class InteractionZone : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent OnInteract { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            
            if (ReferenceEquals(null, player))
            {
                return;
            }
            
            player.InteractionButton.OnInteractionZoneEnter(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            
            if (ReferenceEquals(null, player))
            {
                return;
            }
            
            player.InteractionButton.OnInteractionZoneExit(this);
        }
    }
}