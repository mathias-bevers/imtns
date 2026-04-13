using System;
using CleanRoom.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour, IGameStateObject
    {
        [SerializeField] private Image isopropylStain;
        [SerializeField] private DirtPiece dirtPrefab;
        [SerializeField] private DragAndSnap isopropyl;
        [SerializeField] private float sprayRadius;

        private float distance = 0;
        public bool IsSprayed { get; private set; } = false;
        private Transform cachedTransform = null;

        private void Awake()
        {
            cachedTransform = transform;
            cachedTransform.GetComponentInParents<GameState>().AddStateObject(this);
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
            }
        }

        private void CheckIsopropyl()
        {
            if (IsSprayed)
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
            IsSprayed = true;
            
            isopropylStain.gameObject.SetActive(true);
            isopropylStain.transform.SetAsLastSibling();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, sprayRadius);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 210, 90), "iso dist: " + distance);
        }
    }
}