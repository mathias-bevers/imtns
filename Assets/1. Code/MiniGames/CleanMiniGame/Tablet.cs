using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour
    {
        [SerializeField] private DirtPiece dirtPrefab;
        [SerializeField] private DragAndSnap isopropyl;
        
        private Transform cachedTransform;
        public Transform CachedTransform
        {
            get
            {
                cachedTransform ??= transform;
                return cachedTransform;
            }
        }

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
    }
}