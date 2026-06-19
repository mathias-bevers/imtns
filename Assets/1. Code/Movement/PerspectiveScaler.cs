using NaughtyAttributes;
using UnityEngine;

public class PerspectiveScaler : MonoBehaviour
{
    [Header("x is min and y is max"), SerializeField]
    
    private Vector2 yRange;
    [SerializeField] private Vector2 scaleRange;

    private Transform cachedTransform;


    private void Awake()
    {
        cachedTransform = transform;
    }

    private void LateUpdate()
    {
        Scale(cachedTransform);
    }

    [Button("Scale")]
    private void ScaleEditor()
    {
        float yPosition = transform.position.y;
        if (yPosition < yRange.x || yPosition > yRange.y)
        {
            Debug.LogError("player is outside the accepted range");
            return;
        }

        Scale(transform);
    }

    private void Scale(Transform target)
    {
        float yDifference = yRange.y - yRange.x;
        float scaleDifference = scaleRange.y - scaleRange.x;

        float yPercent = (yRange.y - target.position.y) / yDifference;
        yPercent = Mathf.Clamp01(yPercent);
        float scale = scaleRange.x + scaleDifference * yPercent;
        target.localScale = new Vector3(scale, scale, 1);
    }
}