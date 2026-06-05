using System;
using CleanRoom.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DirtPiece : MonoBehaviour
    {
        private const string ALREADY_COMPLETED_MESSAGE = "Je hebt deze stap al voltooid";
        private const string NOT_READY_YET = "Je bent een of meerdere stappen vergeten";

        [SerializeField] private float cleanDistanceBase;
        [SerializeField] private Sprite[] sprites;

        private string stateName;
        private GameManager gameManager;
        private CleanGame manager;
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

        public void Initialize(float scale, Vector2 position)
        {
            cachedTransform = (RectTransform)transform;
            image = GetComponent<Image>();
            
            stateName = KattenKasteel.FSM.StateMachine.Instance.ActiveState.StateName;
            manager = CleanGame.Instance;
            gameManager = GameManager.Instance;

            image.sprite = sprites.GetRandomElement();

            cachedTransform.sizeDelta *= scale;
            cachedTransform.anchoredPosition = position;
            cleanDistance = cleanDistanceBase * scale;
        }

        private void CheckWipe()
        {
            distance = Vector2.Distance(cachedTransform.position, manager.Wipe.CachedTransform.position);

            if (distance > cleanDistance)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}