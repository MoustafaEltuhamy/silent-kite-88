using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AGAPI.Foundation
{
    [AddComponentMenu("Layout/Stretch Grid Layout Group")]
    public class StretchGridLayoutGroup : GridLayoutGroup
    {
        public enum FitType
        {
            Uniform,       // rows and columns ~ square
            Width,         // user sets columns, rows auto
            Height,        // user sets rows, columns auto
            FixedRows,     // alias for Height
            FixedColumns   // alias for Width
        }

        [Tooltip("Controls how rows & columns are determined.")]
        public FitType fitType = FitType.Uniform;

        [Tooltip("Base cell size before uniform scaling to fit container.\n" +
                  "Initial width:height ratio used for sizing.")]
        public Vector2 initialCellSize = new Vector2(100, 100);

        [Tooltip("Number of rows when using Height or FixedRows.")]
        public int rows = 1;

        [Tooltip("Number of columns when using Width or FixedColumns.")]
        public int columns = 1;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            int childCount = rectChildren.Count;

            // Determine rows & columns based on fitType
            switch (fitType)
            {
                case FitType.Uniform:
                    int sq = Mathf.CeilToInt(Mathf.Sqrt(childCount));
                    rows = sq;
                    columns = sq;
                    break;
                case FitType.Width:
                case FitType.FixedColumns:
                    // columns set in inspector
                    rows = Mathf.CeilToInt(childCount / (float)columns);
                    break;
                case FitType.Height:
                case FitType.FixedRows:
                    // rows set in inspector
                    columns = Mathf.CeilToInt(childCount / (float)rows);
                    break;
            }

            // Enforce GridLayoutGroup constraint so Unity does NOT override your intended layout.
            switch (fitType)
            {
                case FitType.Width:
                case FitType.FixedColumns:
                    constraint = Constraint.FixedColumnCount;
                    constraintCount = Mathf.Max(1, columns);
                    break;

                case FitType.Height:
                case FitType.FixedRows:
                    constraint = Constraint.FixedRowCount;
                    constraintCount = Mathf.Max(1, rows);
                    break;

                case FitType.Uniform:
                    constraint = Constraint.FixedColumnCount;
                    constraintCount = Mathf.Max(1, columns); // after you compute sq
                    break;
            }

            // Calculate available space
            float totalPaddingX = padding.left + padding.right + spacing.x * (columns - 1);
            float totalPaddingY = padding.top + padding.bottom + spacing.y * (rows - 1);
            float availableWidth = rectTransform.rect.width - totalPaddingX;
            float availableHeight = rectTransform.rect.height - totalPaddingY;

            // Compute scale factors based on initialCellSize and preserve aspect ratio
            float widthScale = initialCellSize.x > 0
                ? availableWidth / (initialCellSize.x * columns)
                : float.MaxValue;
            float heightScale = initialCellSize.y > 0
                ? availableHeight / (initialCellSize.y * rows)
                : float.MaxValue;
            // use the smaller scale to fit both dimensions
            float scale = Mathf.Min(widthScale, heightScale);

            // Final uniformly scaled cell size preserves aspect ratio
            cellSize = initialCellSize * scale;
        }

        public override void SetLayoutHorizontal()
        {
            base.SetLayoutHorizontal();
        }

        public override void SetLayoutVertical()
        {
            base.SetLayoutVertical();
        }

        public void SetGridSize(Vector2Int gridSize)
        {
            if (fitType == FitType.FixedColumns || fitType == FitType.Width)
            {
                columns = gridSize.x;
            }
            else if (fitType == FitType.FixedRows || fitType == FitType.Height)
            {
                rows = gridSize.y;
            }

            //Refresh Layout Group
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}

