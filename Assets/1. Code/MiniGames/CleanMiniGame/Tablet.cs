using System;
using System.Collections.Generic;
using System.Linq;
using CleanRoom.Menus;
using KattenKasteel.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour
    {
        public enum CleanlinessLevel
        {
            UnExposed,
            Dirty,
            Sprayed,
            Cleaned,
            Bagged
        }

        private const string ALREADY_COMPLETED_MESSAGE = "Je hebt deze stap al voltooid";
        private const string NOT_READY_YET = "Je bent een of meerdere stappen vergeten";

        [SerializeField] private float interactionRadius;
        [SerializeField] private DirtPiece dirtPrefab;
        [SerializeField] private DragAndSnap uvLight;
        [SerializeField] private DragAndSnap isopropyl;
        [SerializeField] private DragAndSnap bag;
        [SerializeField] private GameObject baggedTablet;

        public CleanlinessLevel Cleanliness { get; private set; } = CleanlinessLevel.UnExposed;
        private Dictionary<DragAndSnap, CleanlinessLevel> itemLevelPairs = null;
        private Bounds bounds;

        private GameManager gameManager;
        private CleanGame manager = null;
        private RectTransform cachedTransform = null;
        private string stateName;
        private Transform isopropylStain;

        private void Start()
        {
            cachedTransform = (RectTransform)transform;
            isopropylStain = cachedTransform.GetChild(0);
            manager = CleanGame.Instance;
            gameManager = GameManager.Instance;
            stateName = StateMachine.Instance.ActiveState.StateName;
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
                float scale = Random.Range(50, 111) * 0.01f;
                Vector2 position = Random.insideUnitCircle * 150;

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
                    StateMachine.Instance.CompleteActiveState();
                    MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent += OnCompletePopupClose;
                    break;
                case CleanlinessLevel.UnExposed:
                default: throw new ArgumentOutOfRangeException();
            }
        }

        // private void CheckIsopropyl()
        // {
        //     if (!bounds.Contains(isopropyl.CachedTransform.position))
        //     {
        //         return;
        //     }
        //
        //     if (Cleanliness != CleanlinessLevel.Dirty)
        //     {
        //         manager.ShowError(CleanlinessLevel.Dirty, Cleanliness < CleanlinessLevel.Dirty);
        //         return;
        //     }
        //
        //     isopropyl.SnapAndDisableTemporarily();
        //     Cleanliness = CleanlinessLevel.Sprayed;
        //
        //     isopropylStain.gameObject.SetActive(true);
        //     isopropylStain.SetAsLastSibling();
        // }
        //
        // private void CheckWipe()
        // {
        //     if (!bounds.Contains(manager.Wipe.CachedTransform.position))
        //     {
        //         return;
        //     }
        //
        //     if (Cleanliness == CleanlinessLevel.Sprayed)
        //     {
        //         return;
        //     }
        //
        //     manager.ShowError(CleanlinessLevel.Sprayed, Cleanliness < CleanlinessLevel.Sprayed);
        // }
        //
        // private void CheckBag()
        // {
        //     if (!bounds.Contains(bag.CachedTransform.position))
        //     {
        //         return;
        //     }
        //
        //     switch (Cleanliness)
        //     {
        //         case < CleanlinessLevel.Cleaned:
        //             gameManager.OnMistakeMade(stateName, NOT_READY_YET);
        //             return;
        //     }
        //
        //
        //     Cleanliness = CleanlinessLevel.Bagged;
        //     bag.SnapAndDisable();
        //     baggedTablet.SetActive(true);
        //     bag.gameObject.SetActive(false);
        //     gameObject.SetActive(false);
        //     StateMachine.Instance.CompleteActiveState();
        //     MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent += OnCompletePopupClose;
        // }

        // private void CheckUV()
        // {
        //     if (!bounds.Contains(uvLight.CachedTransform.position))
        //     {
        //         return;
        //     }
        //
        //     if (Cleanliness >= CleanlinessLevel.Dirty)
        //     {
        //         GameManager.Instance.OnMistakeMade(stateName, ALREADY_COMPLETED_MESSAGE);
        //         return;
        //     }
        //
        //     Cleanliness = CleanlinessLevel.Dirty;
        //     uvLight.SnapAndDisableTemporarily();
        //     SpawnDirt();
        // }

        private void OnCompletePopupClose(Popup.MessageType messageType)
        {
            if (messageType != Popup.MessageType.CompletedMiniGame)
            {
                return;
            }

            manager.OnCompleteTransition.Transition();
            MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent -= OnCompletePopupClose;
        }
    }
}