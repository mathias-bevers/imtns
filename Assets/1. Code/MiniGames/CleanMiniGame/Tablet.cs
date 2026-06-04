using CleanRoom.Menus;
using UnityEngine;
using KattenKasteel.FSM;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour
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
        private CleanGame manager = null;
        private float distance = 0;

        private string stateName;
        private Transform isopropylStain;
        private Transform cachedTransform = null;

        private void Start()
        {
            cachedTransform = transform;
            isopropylStain = cachedTransform.GetChild(0);
            manager = CleanGame.Instance;
            stateName = StateMachine.Instance.ActiveState.StateName;
        }

        private void Update()
        {
            CheckIsopropyl();
            CheckBag();
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

            Cleanliness = CleanlinessLevel.Cleaned;
            manager.Wipe.gameObject.SetActive(false);
            isopropylStain.gameObject.SetActive(false);
            manager.WipeBox.PreventInvoke = true;
            manager.BagDispenser.PreventInvoke = false;
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
            manager.BagDispenser.PreventInvoke = true;
            StateMachine.Instance.CompleteActiveState();
            
            PopupMenu popupMenu = MenuManager.Instance.GetMenuOfType<PopupMenu>();
            popupMenu.CreatePopup("Je hebt deze minigame voltooit", Popup.MessageType.CompletedMiniGame, stateName);
            popupMenu.Popup.closeEvent += OnCompletePopupClose;
        }

        private void OnCompletePopupClose(Popup.MessageType messageType)
        {
            if (messageType != Popup.MessageType.CompletedMiniGame)
            {
                return;
            }
            
            manager.OnCompleteTransition.Transition();
            MenuManager.Instance.GetMenuOfType<PopupMenu>().Popup.closeEvent -= OnCompletePopupClose;
        }

        private bool InInteractionRadius(Vector2 otherPosition)
        {
            distance = Vector2.Distance(otherPosition, cachedTransform.position);
            return distance <= interactionRadius;
        }
    }
}