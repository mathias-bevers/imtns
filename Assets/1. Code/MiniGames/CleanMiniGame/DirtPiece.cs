using System;
using CleanRoom.Menus;
using CleanRoom.StateMachine;
using UnityEngine;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DirtPiece : MonoBehaviour
    {
        private const string NO_SPRAY_WARNING =
            "Zorg er voor dat je eerst de isopropyl gebruikt";
        
        private CleanGameState state;
        private DragAndSnap wipe;

        [SerializeField] private float cleanDistanceBase;
        private float cleanDistance;
        private float distance;
        private RectTransform cachedTransform;

        private void OnDestroy()
        {
            destroyedEvent?.Invoke();
        }

        private void Update()
        {
            CheckWipe();
        }

        public event Action destroyedEvent;

        public void Initialize(float scale, Vector2 position)
        {
            cachedTransform = (RectTransform)transform;

            state = StateMachine.StateMachine.Instance.ActiveState as CleanGameState;
            wipe = state?.Wipe;

            cachedTransform.sizeDelta *= scale;
            cachedTransform.anchoredPosition = position;
            cleanDistance = cleanDistanceBase * scale;
        }

        private void CheckWipe()
        {
            distance = Vector2.Distance(cachedTransform.position,
                wipe.CachedTransform.position);

            if (distance > cleanDistance)
            {
                return;
            }

            if (state.Tablet.Cleanliness < Tablet.CleanlinessLevel.Sprayed)
            {
                MenuManager.Instance.GetMenuOfType<PopupMenu>()
                    .CreatePopup(NO_SPRAY_WARNING, Popup.Level.Warning);
                wipe.OnEndDrag(null);
                return;
            }
            
            Destroy(gameObject);
        }
    }
}