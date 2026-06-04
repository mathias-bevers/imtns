using System;
using CleanRoom.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DirtPiece : MonoBehaviour
    {
        private const string NO_SPRAY_WARNING = "Zorg er voor dat je eerst de isopropyl gebruikt";

        [SerializeField] private float cleanDistanceBase;
        [SerializeField] private Sprite[] sprites;

        private string stateName;
        private CleanGame state;
        private DragAndSnap wipe;
        private float cleanDistance;
        private float distance;

        private Image image;
        private RectTransform cachedTransform;

        private void Update()
        {
            CheckWipe();
        }

        private void OnDestroy()
        {
            destroyedEvent?.Invoke();
        }

        public event Action destroyedEvent;
        public event Action<string, string> mistakeMadeEvent;

        public void Initialize(float scale, Vector2 position)
        {
            cachedTransform = (RectTransform)transform;
            image = GetComponent<Image>();
            

            stateName = KattenKasteel.FSM.StateMachine.Instance.ActiveState.StateName;
            wipe = state?.Wipe;

            image.sprite = sprites.GetRandomElement();

            cachedTransform.sizeDelta *= scale;
            cachedTransform.anchoredPosition = position;
            cleanDistance = cleanDistanceBase * scale;
        }

        private void CheckWipe()
        {
            distance = Vector2.Distance(cachedTransform.position, wipe.CachedTransform.position);

            if (distance > cleanDistance)
            {
                return;
            }

            if (state.Tablet.Cleanliness < Tablet.CleanlinessLevel.Sprayed)
            {
                mistakeMadeEvent?.Invoke(stateName, NO_SPRAY_WARNING);

                wipe.OnEndDrag(null);
                return;
            }

            Destroy(gameObject);
        }
    }
}