using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public class UguiGrid : UguiObject<GridLayoutGroup>, IGrid {

        private int limit;


        public float CellWidth
        {
            get => component.cellSize.x;
            set => component.cellSize = new Vector2(value, component.cellSize.y);
        }

        public float CellHeight
        {
            get => component.cellSize.y;
            set => component.cellSize = new Vector2(component.cellSize.x, value);
        }

        public Vector2 CellSize
        {
            get => component.cellSize;
            set => component.cellSize = value;
        }

        public float SpaceWidth
        {
            get => component.spacing.x;
            set => component.spacing = new Vector2(value, component.spacing.y);
        }

        public float SpaceHeight
        {
            get => component.spacing.y;
            set => component.spacing = new Vector2(component.spacing.x, value);
        }

        public Vector2 Spacing
        {
            get => component.spacing;
            set => component.spacing = value;
        }

        public GridLayoutGroup.Corner Corner
        {
            get => component.startCorner;
            set => component.startCorner = value;
        }

        public GridLayoutGroup.Axis Axis
        {
            get => component.startAxis;
            set
            {
                component.startAxis = value;
                RefreshConstraint();
            }
        }

        public TextAnchor Alignment
        {
            get => component.childAlignment;
            set => component.childAlignment = value;
        }

        public int Limit
        {
            get => limit;
            set
            {
                component.constraintCount = limit = Mathf.Clamp(value, 0, int.MaxValue);
                RefreshConstraint();
            }
        }


        protected override void Awake()
        {
            base.Awake();

            Limit = 1;
        }

        /// <summary>
        /// Re-evaluates the grid's constraint value.
        /// </summary>
        private void RefreshConstraint()
        {
            if (limit == 0)
                component.constraint = GridLayoutGroup.Constraint.Flexible;
            else
            {
                component.constraint = component.startAxis == GridLayoutGroup.Axis.Vertical ?
                    GridLayoutGroup.Constraint.FixedColumnCount :
                    GridLayoutGroup.Constraint.FixedRowCount;
            }
        }
    }
}