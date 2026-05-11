using UnityEngine.UI;

namespace CleanRoom.Interactables
{
    public class InteractionButton : Button
    {
        private InteractionZone interactionZone;

        public void OnInteractionZoneEnter(InteractionZone interactionZone)
        {
            this.interactionZone = interactionZone;
            onClick.AddListener(this.interactionZone.OnInteract.Invoke);
        }

        public void OnInteractionZoneExit(InteractionZone interactionZone)
        {
            this.interactionZone = interactionZone;
            onClick.RemoveListener(this.interactionZone.OnInteract.Invoke);
        }
    }
}