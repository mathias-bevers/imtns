using CleanRoom.StateMachine;
using CleanRoom.StateMachine.GameStates;
using UnityEngine;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DirtPiece : MonoBehaviour, IGameStateObject
    {
        private static CleanItemMiniGameState _state;
        private static Wipe _wipe;

        [SerializeField] private float cleanDistanceBase;

        public RectTransform CachedTransform { get; private set; }

        private float scale;
        private float cleanDistance;
        private float distance;

        public void Tick(float deltaTime)
        {
            Debug.Log("dirt tick");

            distance = Vector2.Distance(CachedTransform.position, _wipe.transform.position);

            if (distance > cleanDistance)
            {
                return;
            }

            _state.RemoveStateObject(this);
            Destroy(gameObject);
        }

        public void FixedTick(float fixedDeltaTime) { }

        public void Initialize(float scale, Vector2 position)
        {
            this.scale = scale;
            CachedTransform = (RectTransform)transform;

            if (ReferenceEquals(null, _state))
            {
                _state = GameStateController.Instance.GetGameState<CleanItemMiniGameState>();
                _wipe = _state.Wipe;
            }
            
            _state.AddStateObject(this);
            
            CachedTransform.sizeDelta *= scale;
            CachedTransform.anchoredPosition = position;
            cleanDistance = cleanDistanceBase * scale;
        }
    }
}