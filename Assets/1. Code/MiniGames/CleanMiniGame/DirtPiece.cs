using System;
using CleanRoom.Menus;
using CleanRoom.StateMachine;
using CleanRoom.StateMachine.GameStates;
using UnityEngine;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DirtPiece : MonoBehaviour, IGameStateObject
    {
        private const string NO_SPRAY_WARNING =
            "Zorg er voor dat je eerst de isopropyl gebruikt";
        
        private static CleanItemMiniGameState _state;
        private static DragAndSnap _wipe;

        [SerializeField] private float cleanDistanceBase;
        private float cleanDistance;
        private float distance;
        private RectTransform cachedTransform;

        private void OnDestroy()
        {
            destroyedEvent?.Invoke();
        }

        public void Tick(float deltaTime)
        {
            CheckWipe();
        }

        public void FixedTick(float fixedDeltaTime) { }

        public event Action destroyedEvent;

        public void Initialize(float scale, Vector2 position)
        {
            cachedTransform = (RectTransform)transform;

            if (ReferenceEquals(null, _state))
            {
                _state = StateMachine.StateMachine.Instance
                    .GetGameState<CleanItemMiniGameState>();
                _wipe = _state.Wipe;
            }

            _state.AddStateObject(this);

            cachedTransform.sizeDelta *= scale;
            cachedTransform.anchoredPosition = position;
            cleanDistance = cleanDistanceBase * scale;
        }

        private void CheckWipe()
        {
            distance = Vector2.Distance(cachedTransform.position,
                _wipe.CachedTransform.position);

            if (distance > cleanDistance)
            {
                return;
            }

            if (_state.Tablet.Cleanliness < Tablet.CleanlinessLevel.Sprayed)
            {
                MenuManager.Instance.GetMenuOfType<PopupMenu>()
                    .CreatePopup(NO_SPRAY_WARNING, Popup.Level.Warning);
                _wipe.OnEndDrag(null);
                return;
            }

            _state.RemoveStateObject(this);
            Destroy(gameObject);
        }
    }
}