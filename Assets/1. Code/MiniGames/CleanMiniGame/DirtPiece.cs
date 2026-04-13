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

        public RectTransform CachedTransform { get; private set; }

        private float cleanDistance;
        private float distance;

        public void Tick(float deltaTime)
        {
            CheckWipe();
        }

        public void FixedTick(float fixedDeltaTime) { }

        public void Initialize(float scale, Vector2 position)
        {
            CachedTransform = (RectTransform)transform;

            if (ReferenceEquals(null, _state))
            {
                _state = GameStateController.Instance
                    .GetGameState<CleanItemMiniGameState>();
                _wipe = _state.Wipe;
            }

            _state.AddStateObject(this);

            CachedTransform.sizeDelta *= scale;
            CachedTransform.anchoredPosition = position;
            cleanDistance = cleanDistanceBase * scale;
        }

        private void CheckWipe()
        {
            distance = Vector2.Distance(CachedTransform.position,
                _wipe.CachedTransform.position);

            if (distance > cleanDistance)
            {
                return;
            }

            if (!_state.Tablet.IsSprayed)
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