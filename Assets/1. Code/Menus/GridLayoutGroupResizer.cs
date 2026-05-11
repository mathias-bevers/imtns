using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Menus
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridLayoutGroupResizer : MonoBehaviour
    {
        private RectTransform rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                rectTransform ??= transform as RectTransform;
                return rectTransform;
            }
        }

        private GridLayoutGroup grid;

        public void Resize(int minHeight = 0)
        {
            grid ??= GetComponent<GridLayoutGroup>();
            
            float totalCellWidth = grid.cellSize.x + grid.spacing.x;
            int itemsPerRow = Mathf.FloorToInt(RectTransform.rect.width / totalCellWidth);

            if (itemsPerRow * totalCellWidth + grid.cellSize.x < RectTransform.rect.width)
            {
                ++itemsPerRow;
            }

            int rowCount = Mathf.CeilToInt(RectTransform.childCount / (float)itemsPerRow);
            float calculatedHeight = (rowCount * (grid.cellSize.y + grid.spacing.y)) - grid.spacing.y;
            calculatedHeight = Mathf.Max(calculatedHeight, minHeight);

            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, calculatedHeight);
        }
    }
}