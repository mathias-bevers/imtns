using UnityEngine;

public class TriggerLayerOrder : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField] private SpriteRenderer[] aboveOrderInLayer;
    [SerializeField] private SpriteRenderer[] belowOrderInLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG)) return;

        SetSortingOrders(6, 0);
    }

    private void SetSortingOrders(int aboveOrder, int belowOrder)
    {
        foreach (var layer in aboveOrderInLayer)
        {
            if (layer != null) layer.sortingOrder = aboveOrder;
        }
        foreach (var layer in belowOrderInLayer)
        {
            if (layer != null) layer.sortingOrder = belowOrder;
        }
    }
}