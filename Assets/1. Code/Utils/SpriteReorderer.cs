using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace CleanRoom.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteReorderer : MonoBehaviour
    {
        private const int GIZMO_X = 100;

        [SerializeField] private float gizmoOffset = 0;
        [SerializeField] private Vector2 range;
        [SerializeField] private int layers;

        private float rangeDifference;
        private new SpriteRenderer renderer;
        private Transform cachedTransform;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            cachedTransform = transform;
            rangeDifference = range.y - range.x;
        }

        private void LateUpdate()
        {
            int layer = CalculateLayer();
            
            if (layer == renderer.sortingOrder)
            {
                return;
            }

            renderer.sortingOrder = layer;
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
            Gizmos.DrawLine(new Vector3(-GIZMO_X, range.x + gizmoOffset), new Vector3(GIZMO_X, range.x + gizmoOffset));
            Gizmos.color = Color.limeGreen;
            Gizmos.DrawLine(new Vector3(-GIZMO_X, range.y + gizmoOffset), new Vector3(GIZMO_X, range.y + gizmoOffset));

            Gizmos.color = Color.cyan;
            for (int i = 1; i < layers; ++i)
            {
                float y = range.x + (stepSize * i) + gizmoOffset;
                Gizmos.DrawLine(new Vector3(-GIZMO_X, y, 0), new Vector3(GIZMO_X, y, 0));
            }
        }
    }
}