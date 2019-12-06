namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Abstraction of a grid which aligns objects at a fixed interval of rows and columns.
    /// </summary>
    public interface IGrid {
    
        /// <summary>
        /// Distance between each element in y axis.
        /// </summary>
        float RowDistance { get; set; }

        /// <summary>
        /// Distance between each object in x axis.
        /// </summary>
        float ColumnDistance { get; set; }

        /// <summary>
        /// Max number of objects to be aligned in a single direction before breaking to a new line.
        /// Default = 0
        /// </summary>
        int DirectionLimit { get; set; }

        /// <summary>
        /// Direction of the object alignment.
        /// </summary>
        Directions Direction { get; set; }

        /// <summary>
        /// Pivot point of the grid where the object will be aligned from.
        /// </summary>
        Pivots Pivot { get; set; }
    }
}