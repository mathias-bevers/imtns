using System;
using System.Collections.Generic;
using System.Linq;
using CleanRoom.Menus;
using CleanRoom.Utils;
using KattenKasteel.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour
    {
        private static readonly Vector2Int DIRT_SCALE = new (50, 111);
        
        public enum CleanlinessLevel
        {
            UnExposed,
            Dirty,
            Sprayed,
            Cleaned,
            Bagged
        }
        
        [SerializeField] private DirtPiece dirtPrefab;
        [SerializeField] private DragAndSnap uvLight;
        [SerializeField] private DragAndSnap isopropyl;
        [SerializeField] private DragAndSnap bag;
        [SerializeField] private GameObject baggedTablet;

        public CleanlinessLevel Cleanliness { get; private set; } = CleanlinessLevel.UnExposed;
        private Dictionary<DragAndSnap, CleanlinessLevel> itemLevelPairs = null;
        private Bounds bounds;

        private CleanGame manager = null;
        private RectTransform cachedTransform = null;
        private Transform isopropylStain;

        private void Start()
        {
            cachedTransform = (RectTransform)transform;
            isopropylStain = cachedTransform.GetChild(0);
            manager = CleanGame.Instance;
            itemLevelPairs = new Dictionary<DragAndSnap, CleanlinessLevel>()
            {
                { uvLight, CleanlinessLevel.UnExposed },
                { isopropyl, CleanlinessLevel.Dirty },
                { manager.Wipe, CleanlinessLevel.Sprayed },
                { bag, CleanlinessLevel.Cleaned }
            };

            Rect rect = cachedTransform.rect;
            bounds = new Bounds(cachedTransform.position, new Vector3(rect.width * 0.5f, rect.height * 0.5f, 1));
            bounds.size *= 0.75f;
        }

        private void Update()
        {
            CheckStages();
        }

        public void SpawnDirt()
        {
            for (int i = 0; i < cachedTransform.childCount - 1; ++i)
            {
                Destroy(cachedTransform.GetChild(i).gameObject);
            }

            int dirtCount = Random.Range(3, 6);

            for (int i = 0; i < dirtCount; ++i)
            {
                float scale = Random.Range(DIRT_SCALE.x, DIRT_SCALE.y) * 0.01f;
                Vector2 position = bounds.GetRandomPointInBounds();

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

            NextStage();
        }

        private void CheckStages()
        {
            foreach (KeyValuePair<DragAndSnap, CleanlinessLevel> kvp in itemLevelPairs.Where(kvp =>
                         bounds.Contains(kvp.Key.CachedTransform.position)))
            {
                if (Cleanliness != kvp.Value)
                {
                    kvp.Key.SnapAndDisableTemporarily();
                    manager.ShowError(kvp.Value, Cleanliness < kvp.Value);
                    break;
                }
                
                if (Cleanliness == CleanlinessLevel.Sprayed)
                {
                    continue;
                }

                kvp.Key.SnapAndDisableTemporarily();
                NextStage();
            }
        }

        private void NextStage()
        {
            ++Cleanliness;
            FlashOverlay.Instance.PlayAnimation(true);

            switch (Cleanliness)
            {
                case CleanlinessLevel.Dirty:
                    SpawnDirt();
                    break;
                case CleanlinessLevel.Sprayed:
                    isopropylStain.gameObject.SetActive(true);
                    isopropylStain.SetAsLastSibling();
                    break;
                case CleanlinessLevel.Cleaned:
                    manager.Wipe.SnapAndDisableTemporarily();
                    isopropylStain.gameObject.SetActive(false);
                    break;
                case CleanlinessLevel.Bagged:
                    baggedTablet.SetActive(true);
                    bag.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    manager.CompleteMiniGame();
                    break;
                case CleanlinessLevel.UnExposed:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}