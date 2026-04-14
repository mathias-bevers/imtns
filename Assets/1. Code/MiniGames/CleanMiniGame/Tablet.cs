using CleanRoom.StateMachine;
using CleanRoom.StateMachine.GameStates;
using UnityEngine;

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
        
        [SerializeField] private float interactionRadius;
        [SerializeField] private DirtPiece dirtPrefab;
        [SerializeField] private DragAndSnap isopropyl;
        [SerializeField] private DragAndSnap bag;
        [SerializeField] private GameObject baggedTablet;
        
        public CleanlinessLevel Cleanliness { get; private set; } = CleanlinessLevel.Dirty;
        private CleanItemMiniGameState state = null;
        private float distance = 0;

        private Transform isopropylStain;
        private Transform cachedTransform = null;

        private void Awake()
        {
            cachedTransform = transform;
            isopropylStain = cachedTransform.GetChild(0);
            state = cachedTransform.GetComponentInParents<CleanItemMiniGameState>();
            state.AddStateObject(this);
        }

        public void Tick(float deltaTime)
        {
            CheckIsopropyl();
            CheckBag();
        }

        public void FixedTick(float fixedDeltaTime) { }

        public void SpawnDirt()
        {
            int dirtCount = Random.Range(3, 6);

            for (int i = 0; i < dirtCount; ++i)
            {
                float scale = Random.Range(50, 111) * 0.01f;
                Vector2 position = Random.insideUnitCircle * 201;
                
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
            isopropylStain.gameObject.SetActive(false);
            state.WipeBox.PreventInvoke = true;
            state.BagDispenser.PreventInvoke = false;
        }

        private void CheckIsopropyl()
        {
            if (Cleanliness >= CleanlinessLevel.Sprayed ||
                !InInteractionRadius(isopropyl.CachedTransform.position))
            {
                return;
            }

            isopropyl.SnapAndDisable();
            Cleanliness = CleanlinessLevel.Sprayed;
            distance = -1;

            isopropylStain.gameObject.SetActive(true);
            isopropylStain.SetAsLastSibling();
        }

        private void CheckBag()
        {
            if (Cleanliness != CleanlinessLevel.Cleaned ||
                !InInteractionRadius(bag.CachedTransform.position))
            {
                return;
            }
            
            bag.SnapAndDisable();
            baggedTablet.SetActive(true);
            bag.gameObject.SetActive(false);
            gameObject.SetActive(false);
            state.BagDispenser.PreventInvoke = true;
            
            //TODO: set state as completed
        }

        private bool InInteractionRadius(Vector2 otherPosition)
        {
            distance = Vector2.Distance(otherPosition, cachedTransform.position);
            return distance <= interactionRadius;
        }
    }
}