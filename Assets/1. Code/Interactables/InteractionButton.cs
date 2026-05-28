using UnityEngine.UI;

namespace CleanRoom.Interactables
{
    public class InteractionButton : Button
    {
        private InteractionZone interactionZone;

        protected override void Awake()
        {
            base.Awake();
            interactable = false;
        }

        public void OnInteractionZoneEnter(InteractionZone interactionZone)
        {
            this.interactionZone = interactionZone;
            onClick.AddListener(this.interactionZone.OnInteract.Invoke);
            interactable = true;
        }

        public void OnInteractionZoneExit(InteractionZone interactionZone)
        {
            onClick.RemoveListener(interactionZone.OnInteract.Invoke);

            if (!ReferenceEquals(interactionZone, this.interactionZone))
            {
                return;
            }

            interactable = false;
            this.interactionZone = null;
        }
    }
}