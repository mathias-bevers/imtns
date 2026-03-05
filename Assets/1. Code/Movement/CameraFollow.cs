using UnityEngine;

namespace CleanRoom.Movement
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private bool shouldFollow = true;
        [Tooltip("ldur"), SerializeField] private Vector4 boundingBox;

        private Transform cachedTransform = null;
        private Vector3 offset = Vector3.zero;

        private void Awake()
        {
            cachedTransform = transform;
            offset = cachedTransform.position - playerTransform.position;
        }

        private void LateUpdate()
        {
            if (!shouldFollow)
            {
                return;
            }

            Vector3 targetPosition = playerTransform.position + offset;
            cachedTransform.position = new Vector3(Mathf.Clamp(targetPosition.x, boundingBox.x, boundingBox.w),
                Mathf.Clamp(targetPosition.y, boundingBox.y, boundingBox.z), cachedTransform.position.z);
        }

        private void OnGUI()
        {
            // string message = string.Concat(cachedTransform.position, '\n', boundingBox.min, '\n', boundingBox.max);
            GUI.Label(new Rect(10, 10, 150, 600), cachedTransform.position.ToString());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            // left border
            Gizmos.DrawLine(new Vector2(boundingBox.x, boundingBox.z), new Vector2(boundingBox.x, boundingBox.y));

            // up border
            Gizmos.DrawLine(new Vector2(boundingBox.x, boundingBox.y), new Vector2(boundingBox.w, boundingBox.y));

            // down border
            Gizmos.DrawLine(new Vector2(boundingBox.x, boundingBox.z), new Vector2(boundingBox.w, boundingBox.z));

            // right border
            Gizmos.DrawLine(new Vector2(boundingBox.w, boundingBox.z), new Vector2(boundingBox.w, boundingBox.y));
        }
    }
}