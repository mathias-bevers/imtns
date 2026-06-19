using UnityEngine;

namespace CleanRoom.Utils
{
    public class SpriteReorderer : MonoBehaviour
    {
        private const int GIZMO_X = 100;

        [SerializeField] private Vector2 range;
        [SerializeField] private int layers;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private float rangeDifference;
        private Transform cachedTransform;

        private void Awake()
        {
            try
            {
                spriteRenderer.GetComponentIndex();
            }
            catch (MissingComponentException e)
            {
                Debug.LogError(e.Message + " Disabling component...");
                enabled = false;
                return;
            }

            cachedTransform = transform;
            rangeDifference = range.y - range.x;
        }

        private void LateUpdate()
        {
            int layer = CalculateLayer();

            if (layer == spriteRenderer.sortingOrder)
            {
                return;
            }

            spriteRenderer.sortingOrder = layer;
        }

        private int CalculateLayer()
        {
            float topDifference = range.y - cachedTransform.position.y;
            float percent = topDifference / rangeDifference;
            percent = Mathf.Clamp01(percent);

            int layer = Mathf.RoundToInt(layers * percent);
            return layer;
        }

        private void OnDrawGizmosSelected()
        {
            float diff = range.y - range.x;
            float stepSize = diff / layers;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(-GIZMO_X, range.x), new Vector3(GIZMO_X, range.x));
            Gizmos.color = Color.limeGreen;
            Gizmos.DrawLine(new Vector3(-GIZMO_X, range.y), new Vector3(GIZMO_X, range.y));

            Gizmos.color = Color.cyan;
            for (int i = 1; i < layers; ++i)
            {
                float y = range.x + stepSize * i;
                Gizmos.DrawLine(new Vector3(-GIZMO_X, y, 0), new Vector3(GIZMO_X, y, 0));
            }
        }
    }
}