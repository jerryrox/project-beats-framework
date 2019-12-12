using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public interface IGrid : IGraphicObject {
    
        /// <summary>
        /// Width of each cell.
        /// </summary>
        float CellWidth { get; set; }

        /// <summary>
        /// Height of each cell.
        /// </summary>
        float CellHeight { get; set; }
        
        /// <summary>
        /// Size of each cell in vector.
        /// </summary>
        Vector2 CellSize { get; set; }

        /// <summary>
        /// Width space between each cell.
        /// </summary>
        float SpaceWidth { get; set; }

        /// <summary>
        /// Height space between each cell.
        /// </summary>
        float SpaceHeight { get; set; }

        /// <summary>
        /// Spacing between each cell in vector.
        /// </summary>
        Vector2 Spacing { get; set; }

        /// <summary>
        /// Corner relative to the cells where the cells will be aligned from.
        /// </summary>
        GridLayoutGroup.Corner Corner { get; set; }

        /// <summary>
        /// Direction of cell alignment.
        /// </summary>
        GridLayoutGroup.Axis Axis { get; set; }

        /// <summary>
        /// Corner relative to the grid rect where the cells will be positioned at.
        /// </summary>
        TextAnchor Alignment { get; set; }

        /// <summary>
        /// Max number of cells aligned per line in direction.
        /// This may be affected by the grid's rect.
        /// If limit is 0, the constraint is set to Flexible mode.
        /// Default: 0
        /// </summary>
        int Limit { get; set; }
    }
}