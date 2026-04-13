using CleanRoom.StateMachine;
using CleanRoom.StateMachine.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour, IGameStateObject
    {
        public enum CleanlinessLevel
        {
            Dirty,
            Sprayed,
            Cleaned,
            Bagged
        }

        [SerializeField] private Image isopropylStain;
        [SerializeField] private DirtPiece dirtPrefab;
        [SerializeField] private DragAndSnap isopropyl;
        [SerializeField] private float sprayRadius;

        public CleanlinessLevel Cleanliness { get; private set; } = CleanlinessLevel.Dirty;

        private CleanItemMiniGameState state = null;
        private float distance = 0;
        private Transform cachedTransform = null;

        private void Awake()
        {
            cachedTransform = transform;
            state = cachedTransform.GetComponentInParents<CleanItemMiniGameState>();
            state.AddStateObject(this);
        }

        public void Tick(float deltaTime)
        {
            CheckIsopropyl();
        }

        public void FixedTick(float fixedDeltaTime) { }

        public void SpawnDirt()
        {
            int dirtCount = Random.Range(3, 6);

            for (int i = 0; i < dirtCount; ++i)
            {
                float scale = Random.Range(50, 111) * 0.01f;
                Vector2 position = Random.insideUnitCircle * 401;

                DirtPiece dirtPiece = Instantiate(dirtPrefab, transform);
                dirtPiece.Initialize(scale, position);
                dirtPiece.destroyedEvent += OnDirtPieceDestroyed;
            }
        }

        private void OnDirtPieceDestroyed()
        {
            int piecesLeft = GetComponentsInChildren<DirtPiece>().Length;
            if (piecesLeft > 0)
            {
                return;
            }

            Cleanliness = CleanlinessLevel.Cleaned;
            state.Wipe.gameObject.SetActive(false);
            state.WipeBox.PreventInvoke = true;
            isopropylStain.gameObject.SetActive(false);
        }

        private void CheckIsopropyl()
        {
            if (Cleanliness >= CleanlinessLevel.Sprayed)
            {
                return;
            }

            distance = Vector2.Distance(isopropyl.CachedTransform.position,
                cachedTransform.position);

            if (distance > sprayRadius)
            {
                return;
            }

            isopropyl.SnapAndDisable();
            Cleanliness = CleanlinessLevel.Sprayed;

            isopropylStain.gameObject.SetActive(true);
            isopropylStain.transform.SetAsLastSibling();
        }
    }
}