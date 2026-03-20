using UnityEngine;
using UnityEngine.AdaptivePerformance;

namespace CleanRoom
{
    public class EnvironmentSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        
        [Header("Floors")] [SerializeField] private Transform floorParent;
        [SerializeField] private Vector2 floorOffsetRight;
        [SerializeField] private Vector2 floorOffsetDown;
        [SerializeField] private Sprite floorSprite;

        [Header("Walls")] [SerializeField] private Transform wallParent;
        [SerializeField] private Vector2 wallOffsetRight;
        [SerializeField] private Vector2 wallOffsetUp;
        [SerializeField] private Sprite wall45Sprite;
        [SerializeField] private Sprite wall135Sprite;

        private static readonly Vector3Int FLOOR_SCALE = new(4, 4, 1);
        private static readonly Vector3 WALL_SCALE = new Vector3(3.4f, 3.4f, 1);

        public void Spawn()
        {
            Clear();
            int lastX = gridSize.x - 1;

            for (int y = 0; y < gridSize.y; ++y)
            {
                Vector2 rowStart = floorOffsetDown * y;
                for (int x = 0; x < gridSize.x; ++x)
                {
                    Vector2 spawnPosition = rowStart + (floorOffsetRight * x);
                    SpawnFloor(spawnPosition, $"floor_[{x:00},{y:00}]");

                    if (y == 0)
                    {
                        SpawnWall(spawnPosition, true, $"wallU_[{x:00},{y:00}]");
                    }

                    if (x == lastX)
                    {
                        SpawnWall(spawnPosition, false, $"wallR_[{x:00},{y:00}]");
                    }
                }
            }

            Vector2 offset = CalculateCenter();
            wallParent.position = floorParent.position = -offset;
        }

        private Vector2 CalculateCenter()
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

            Renderer[] renders = floorParent.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; ++i)
            {
                bounds.Encapsulate(renders[i].bounds);
            }

            return bounds.center;
        }
        
        public void Clear()
        {
            wallParent.position = floorParent.position = Vector2.zero;
            floorParent.DestroyAllChildren();
            wallParent.DestroyAllChildren();
        }

        private void SpawnFloor(Vector2 position, string name)
        {
            Transform floorTransform = new GameObject(name).transform;
            floorTransform.SetParent(floorParent);
            floorTransform.localScale = FLOOR_SCALE;
            floorTransform.position = position;

            SpriteRenderer spriteRender = floorTransform.gameObject.AddComponent<SpriteRenderer>();
            spriteRender.sprite = floorSprite;
            spriteRender.sortingOrder = -1;
        }

        private void SpawnWall(Vector2 position, bool isTop, string name)
        {
            Transform wallTransform = new GameObject(name).transform;
            wallTransform.SetParent(wallParent);
            wallTransform.localScale = WALL_SCALE;
            wallTransform.position = position + (isTop ? wallOffsetUp : wallOffsetRight);
            
            SpriteRenderer spriteRenderer = wallTransform.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = isTop ? wall45Sprite : wall135Sprite;
            spriteRenderer.sortingOrder = -1;
        }
    }
}