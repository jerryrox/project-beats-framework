using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;
using PBFramework.Dependencies;

namespace PBFramework.UI
{
    public class UguiListView : UguiScrollView, IListView {

        private Func<IListItem> createItem;
        private Action<IListItem> updateItem;

        private int totalItems;
        private Vector2 cellSize = new Vector2();
        private GridLayoutGroup.Corner corner = GridLayoutGroup.Corner.UpperLeft;
        private GridLayoutGroup.Axis axis = GridLayoutGroup.Axis.Vertical;
        private int limit = 1;

        /// <summary>
        /// List of cell instances currently being pooled.
        /// </summary>
        private List<IListItem> cells = new List<IListItem>();

        /// <summary>
        /// Temporary list of cell instances at the first row/column.
        /// </summary>
        private List<IListItem> firstGroup = new List<IListItem>();

        /// <summary>
        /// Temporary list of cell instances at the last row/column.
        /// </summary>
        private List<IListItem> lastGroup = new List<IListItem>();

        /// <summary>
        /// The direction which the cells will be aligned from the starting corner.
        /// </summary>
        private Vector2 cellDirection = new Vector2();

        /// <summary>
        /// Holds the max holdable number of cells in each direction.
        /// </summary>
        private Vector2Int cellsInDirection = new Vector2Int();

        /// <summary>
        /// Defines the boundary position along the selected axis which triggers cell shifting.
        /// [0] = The bound position toward the negative axis.
        /// [1] = The bound position toward the positive axis.
        /// </summary>
        private float[] shiftBounds = new float[2];

        /// <summary>
        /// Maximum limit of the bound index value.
        /// </summary>
        private int boundIndexLimit;

        /// <summary>
        /// Current shifting boundary index.
        /// </summary>
        private int boundIndex;

        /// <summary>
        /// Whether the cells within the container are sorted toward positive axis.
        /// </summary>
        private bool isSortedToPositive;

        /// <summary>
        /// Whether the listview has been successfully initialized.
        /// </summary>
        private bool isInitialized;


        public int TotalItems
        {
            get => totalItems;
            set
            {
                totalItems = Mathf.Clamp(value, 0, int.MaxValue);
                Recalibrate();
            }
        }

        public float CellWidth
        {
            get => cellSize.x;
            set
            {
                cellSize.x = value;
                CellSize = cellSize;
            }
        }

        public float CellHeight
        {
            get => cellSize.y;
            set
            {
                cellSize.y = value;
                CellSize = cellSize;
            }
        }

        public Vector2 CellSize
        {
            get => cellSize;
            set
            {
                cellSize = value;
                cellSize.x = Mathf.Clamp(value.x, 0f, float.MaxValue);
                cellSize.y = Mathf.Clamp(value.y, 0f, float.MaxValue);
                Recalibrate();
            }
        }

        public GridLayoutGroup.Corner Corner
        {
            get => corner;
            set
            {
                corner = value;
                Recalibrate();
            }
        }

        public GridLayoutGroup.Axis Axis
        {
            get => axis;
            set
            {
                axis = value;
                Recalibrate();
            }
        }

        public int Limit
        {
            get => limit;
            set
            {
                limit = Mathf.Clamp(value, 1, int.MaxValue);
                Recalibrate();
            }
        }

        public Vector2 ContainerStartPos => GetContainerPosAtCorner(this.corner);

        public Vector2 ContainerEndPos
        {
            get
            {
                switch (this.corner)
                {
                    case GridLayoutGroup.Corner.UpperLeft: return GetContainerPosAtCorner(GridLayoutGroup.Corner.LowerRight);
                    case GridLayoutGroup.Corner.UpperRight: return GetContainerPosAtCorner(GridLayoutGroup.Corner.LowerLeft);
                    case GridLayoutGroup.Corner.LowerLeft: return GetContainerPosAtCorner(GridLayoutGroup.Corner.UpperRight);
                    case GridLayoutGroup.Corner.LowerRight: return GetContainerPosAtCorner(GridLayoutGroup.Corner.UpperLeft);
                }
                throw new Exception("Unsupported corner type: " + this.corner);
            }
        }

        /// <summary>
        /// Unsupported property.
        /// </summary>
        public float SpaceWidth { get; set; }

        /// <summary>
        /// Unsupported property.
        /// </summary>
        public float SpaceHeight { get; set; }

        /// <summary>
        /// Unsupported property.
        /// </summary>
        public Vector2 Spacing { get; set; }

        /// <summary>
        /// Unsupported property.
        /// </summary>
        public TextAnchor Alignment { get; set; }

        /// <summary>
        /// Returns whether listview process should be updated.
        /// </summary>
        protected bool ShouldUpdate
        {
            get
            {
                // Requires initialization.
                if (!isInitialized) return false;
                // Cell shifting only occurs when there are more number of items compared to the number of pooled cells.
                if (boundIndexLimit <= 0) return false;
                // Having no cell shouldn't process anything.
                if (cells.Count == 0) return false;
                return true;
            }
        }



        [InitWithDependency]
        private void Init()
        {
            container.Anchor = AnchorType.Center;
            container.Pivot = PivotType.Center;
        }

        public void Initialize(Func<IListItem> createItem, Action<IListItem> updateItem)
        {
            if(createItem == null) throw new ArgumentNullException(nameof(createItem));
            if(updateItem == null) throw new ArgumentNullException(nameof(updateItem));

            this.createItem = createItem;
            this.updateItem = updateItem;

            isInitialized = true;
            Recalibrate();
        }

        public void ForceUpdate()
        {
            if(!isInitialized) return;

            int count = Mathf.Min(cells.Count, totalItems);
            for (int i = 0; i < count; i++)
                UpdateItem(cells[i]);
        }

        public void Recalibrate()
        {
            if(!isInitialized) return;

            // Calculate direction related variables
            CalculateCellDirection();
            CalculateCellsInDirection();

            // Adjust container size.
            var viewportSize = viewport.Size;
            container.Size = new Vector2(
                Mathf.Max(cellsInDirection.x * cellSize.x, viewportSize.x),
                Mathf.Max(cellsInDirection.y * cellSize.y, viewportSize.y)
            );

            // Calculate bounds
            CalculateShiftBounds();

            // Reset cells
            SetupCells();

            // Reset to starting position.
            ResetPosition();
        }

        public IListItem FindItem(Func<IListItem, bool> predicate)
        {
            if(predicate == null) return null;
            
            for (int i = 0; i < cells.Count; i++)
            {
                if(predicate.Invoke(cells[i]))
                    return cells[i];
            }
            return null;
        }

        public override void ResetPosition()
        {
            if(!isInitialized) return;

            // Reset the cells position.
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].ItemIndex = i;
                cells[i].Position = GetItemPosition(i);
            }

            // Reset the container position.
            container.Position = GetContainerPosAtCorner(this.corner);

            // Reset the shifting bound.
            CalculateShiftBounds();

            // Force update the cells.
            ForceUpdate();
        }

        public override void ScrollTo(Vector2 position)
        {
            var startPos = ContainerStartPos;
            var endPos = ContainerEndPos;
            if(startPos.x < endPos.x)
                position.x = Mathf.Clamp(position.x, startPos.x, endPos.x);
            else
                position.x = Mathf.Clamp(position.x, endPos.x, startPos.x);
            if (startPos.y < endPos.y)
                position.y = Mathf.Clamp(position.y, startPos.y, endPos.y);
            else
                position.y = Mathf.Clamp(position.y, endPos.y, startPos.y);
            base.ScrollTo(position);
        }

        protected virtual void Update()
        {
            if(!ShouldUpdate)
                return;

            var curPos = axis == GridLayoutGroup.Axis.Horizontal ? container.Position.x : container.Position.y;

            // Bottom => Top or Left => Right
            if (isSortedToPositive)
            {
                while (curPos < shiftBounds[0] && boundIndex < boundIndexLimit)
                {
                    ShiftBound(true);
                    ShiftFirstToLast();
                }
                while (curPos > shiftBounds[1] && boundIndex > 0)
                {
                    ShiftBound(false);
                    ShiftLastToFirst();
                }
            }
            // Top => Bottom or Right => Left
            else
            {
                while (curPos > shiftBounds[1] && boundIndex < boundIndexLimit)
                {
                    ShiftBound(true);
                    ShiftFirstToLast();
                }
                while (curPos < shiftBounds[0] && boundIndex > 0)
                {
                    ShiftBound(false);
                    ShiftLastToFirst();
                }
            }
        }

        /// <summary>
        /// Shifts bound value to specified direction.
        /// </summary>
        private void ShiftBound(bool isPositive)
        {
            var size = GetCellSizeInDirection() * (isPositive ? 1f : -1f) * (isSortedToPositive ? -1 : 1);
            shiftBounds[0] += size;
            shiftBounds[1] += size;
            boundIndex += isPositive ? 1 : -1;
        }

        /// <summary>
        /// Shifts the first cell group to last.
        /// </summary>
        private void ShiftFirstToLast()
        {
            // Store first and the last group cells.
            for (int i = 0; i < limit; i++)
            {
                firstGroup.Add(cells[i]);
                lastGroup.Add(cells[cells.Count - limit + i]);
            }

            // Shift cells toward the first.
            for (int i = 0; i < cells.Count - limit; i++)
                cells[i] = cells[i + limit];

            // Shift the first to last
            for (int i = 0; i < limit; i++)
            {
                var first = firstGroup[i];
                var last = lastGroup[i];

                // Move first to last.
                cells[cells.Count - limit + i] = first;

                // Make the first item's index come after the last
                first.ItemIndex = last.ItemIndex + limit;

                // Position the item
                if(axis == GridLayoutGroup.Axis.Horizontal)
                    first.X = last.X + cellSize.x * cellDirection.x;
                else
                    first.Y = last.Y + cellSize.y * cellDirection.y;

                // Trigger update on the first item.
                UpdateItem(first);
            }

            // Clear cells in the group list.
            firstGroup.Clear();
            lastGroup.Clear();
        }

        /// <summary>
        /// Shifts the last cell group to first.
        /// </summary>
        private void ShiftLastToFirst()
        {
            // Store first and the last group cells.
            for (int i = 0; i < limit; i++)
            {
                firstGroup.Add(cells[i]);
                lastGroup.Add(cells[cells.Count - limit + i]);
            }

            // Shift cells toward the last.
            for (int i = cells.Count - 1; i >= limit; i--)
                cells[i] = cells[i - limit];

            // Shift the first to last
            for (int i = 0; i < limit; i++)
            {
                var first = firstGroup[i];
                var last = lastGroup[i];

                // Move last to first.
                cells[i] = last;

                // Make the last item's index come before the first
                last.ItemIndex = first.ItemIndex - limit;

                // Position the item
                if(axis == GridLayoutGroup.Axis.Horizontal)
                    last.X = first.X - cellSize.x * cellDirection.x;
                else
                    last.Y = first.Y - cellSize.y * cellDirection.y;

                // Trigger update on the last item.
                UpdateItem(last);
            }

            // Clear cells in the group list.
            firstGroup.Clear();
            lastGroup.Clear();
        }

        /// <summary>
        /// Calculates and caches the cell alignment direction.
        /// </summary>
        private void CalculateCellDirection()
        {
            switch (corner)
            {
                case GridLayoutGroup.Corner.UpperLeft:
                    cellDirection.x = 1f;
                    cellDirection.y = -1f;
                    isSortedToPositive = axis == GridLayoutGroup.Axis.Horizontal;
                    return;
                case GridLayoutGroup.Corner.UpperRight:
                    cellDirection.x = -1f;
                    cellDirection.y = -1f;
                    isSortedToPositive = false;
                    return;
                case GridLayoutGroup.Corner.LowerLeft:
                    cellDirection.x = 1f;
                    cellDirection.y = 1f;
                    isSortedToPositive = true;
                    return;
                case GridLayoutGroup.Corner.LowerRight:
                    cellDirection.x = -1f;
                    cellDirection.y = 1f;
                    isSortedToPositive = axis == GridLayoutGroup.Axis.Vertical;
                    return;
            }
            throw new Exception($"Unknown corner type evaluated: {corner}");
        }

        /// <summary>
        /// Calculates and caches the number of cells in each direction (row, column)
        /// </summary>
        private void CalculateCellsInDirection()
        {
            if (totalItems == 0)
            {
                cellsInDirection.x = cellsInDirection.y = 0;
                return;
            }

            int countInDirection = (totalItems-1) / limit + 1;
            int countInAdjacent = limit;

            if (axis == GridLayoutGroup.Axis.Vertical)
            {
                cellsInDirection.x = countInAdjacent;
                cellsInDirection.y = countInDirection;
            }
            else
            {
                cellsInDirection.x = countInDirection;
                cellsInDirection.y = countInAdjacent;
            }
        }

        /// <summary>
        /// Calculates the cell shifting boundary.
        /// </summary>
        private void CalculateShiftBounds()
        {
            // Creates an initial threshold biased to the direction which the cells are sorted.
            // This makes sure that dragging to the opposite of sorting direction works correctly.
            float positiveSortOffset = (isSortedToPositive ? -1f : 1f);

            // Reset the shifting bounds.
            var posAtCorner = GetContainerPosAtCorner(this.corner);
            if (axis == GridLayoutGroup.Axis.Horizontal)
            {
                shiftBounds[0] = posAtCorner.x + cellSize.x * (-0.5f + positiveSortOffset);
                shiftBounds[1] = posAtCorner.x + cellSize.x * (0.5f + positiveSortOffset);
            }
            else
            {
                shiftBounds[0] = posAtCorner.y + cellSize.y * (-0.5f + positiveSortOffset);
                shiftBounds[1] = posAtCorner.y + cellSize.y * (0.5f + positiveSortOffset);
            }

            // Reset the boundary index.
            boundIndex = 0;
        }

        /// <summary>
        /// Initializes new cells as necessary, or disable excessive ones.
        /// </summary>
        private void SetupCells()
        {
            // Calculate the number of items to be pooled.
            int displayedItems = (Mathf.CeilToInt(GetViewportSizeInDirection() / GetCellSizeInDirection()) + 2) * limit;

            // Create new cells if necessary.
            for (int i = cells.Count; i < displayedItems; i++)
            {
                var item = createItem.Invoke();
                // Enforce same parent
                if((UguiObject)item.Parent != container)
                    item.SetParent(container);
                cells.Add(item);
            }

            // Activate/deactivate cells
            for (int i = 0; i < cells.Count; i++)
                cells[i].Active = i < totalItems;

            // Cache the boundary index limit value.
            boundIndexLimit = Mathf.Max((totalItems - cells.Count + 1) / limit + 1, 0);
        }

        /// <summary>
        /// Updates the specified item via item update event.
        /// </summary>
        private void UpdateItem(IListItem item)
        {
            if (item.ItemIndex < 0 || item.ItemIndex >= totalItems)
            {
                item.Active = false;
                return;
            }

            item.Active = true;
            updateItem.Invoke(item);
        }

        /// <summary>
        /// Returns the position of the item at specified index.
        /// </summary>
        private Vector2 GetItemPosition(int index)
        {
            Vector2 pos = GetFirstCellPosition();
            if (axis == GridLayoutGroup.Axis.Vertical)
            {
                pos.x += cellDirection.x * cellSize.x * (index % limit);
                pos.y += cellDirection.y * cellSize.y * (index / limit);
            }
            else
            {
                pos.x += cellDirection.x * cellSize.x * (index / limit);
                pos.y += cellDirection.y * cellSize.y * (index % limit);
            }
            return pos;
        }

        /// <summary>
        /// Returns the position of the corner from which the cells will be positioned.
        /// </summary>
        private Vector2 GetCornerPosition()
        {
            var size = container.Size;
            switch (corner)
            {
                case GridLayoutGroup.Corner.UpperLeft:
                    return new Vector2(-size.x * 0.5f, size.y * 0.5f);
                case GridLayoutGroup.Corner.UpperRight:
                    return new Vector2(size.x * 0.5f, size.y * 0.5f);
                case GridLayoutGroup.Corner.LowerLeft:
                    return new Vector2(-size.x * 0.5f, -size.y * 0.5f);
                case GridLayoutGroup.Corner.LowerRight:
                    return new Vector2(size.x * 0.5f, -size.y * 0.5f);
            }
            throw new Exception($"Unknown corner type: {corner}");
        }

        /// <summary>
        /// Returns the position of the first cell.
        /// </summary>
        private Vector2 GetFirstCellPosition()
        {
            var cornerPosition = GetCornerPosition();
            cornerPosition.x += cellDirection.x * 0.5f * cellSize.x;
            cornerPosition.y += cellDirection.y * 0.5f * cellSize.y;
            return cornerPosition;
        }

        /// <summary>
        /// Returns the position of the container at the beginning corner.
        /// </summary>
        private Vector2 GetContainerPosAtCorner(GridLayoutGroup.Corner corner)
        {
            var viewportSize = viewport.Size;
            var containerSize = container.Size;
            switch (corner)
            {
                case GridLayoutGroup.Corner.UpperLeft:
                    return new Vector2((containerSize.x - viewport.Size.x) * 0.5f, (containerSize.y - viewportSize.y) * -0.5f);
                case GridLayoutGroup.Corner.UpperRight:
                    return new Vector2((containerSize.x - viewport.Size.x) * -0.5f, (containerSize.y - viewportSize.y) * -0.5f);
                case GridLayoutGroup.Corner.LowerLeft:
                    return new Vector2((containerSize.x - viewport.Size.x) * 0.5f, (containerSize.y - viewportSize.y) * 0.5f);
                case GridLayoutGroup.Corner.LowerRight:
                    return new Vector2((containerSize.x - viewport.Size.x) * -0.5f, (containerSize.y - viewportSize.y) * 0.5f);
            }
            throw new Exception($"Unknown corner type: {corner}");
        }

        /// <summary>
        /// Returns the size of the viewport in the selected direction.
        /// </summary>
        private float GetViewportSizeInDirection()
        {
            if(axis == GridLayoutGroup.Axis.Horizontal)
                return viewport.Size.x;
            return viewport.Size.y;
        }

        /// <summary>
        /// Returns the cell isze in the selected direction.
        /// </summary>
        private float GetCellSizeInDirection()
        {
            if(axis == GridLayoutGroup.Axis.Horizontal)
                return cellSize.x;
            return cellSize.y;
        }
    }
}