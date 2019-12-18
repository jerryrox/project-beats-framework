using UnityEngine;

namespace PBFramework.Graphics.UI
{
    /// <summary>
    /// Template object used to instantiate the popup list view.
    /// </summary>
    public interface IDropdownTemplate : IGraphicObject {

        /// <summary>
        /// Returns the object which represents the template of each cell of the dropdown entry.
        /// </summary>
        GameObject CellObject { get; }

        /// <summary>
        /// Returns the background image of each cell.
        /// </summary>
        GameObject CellBackground { get; }

        /// <summary>
        /// Returns the checkmark image of each cell.
        /// </summary>
        GameObject CellCheckmark { get; }

        /// <summary>
        /// Returns the label which displays the option name for each dropdown entry.
        /// </summary>
        GameObject CellLabel { get; }
    }
}