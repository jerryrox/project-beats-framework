using System;
using UnityEngine;

namespace PBFramework.UI
{
    /// <summary>
    /// Scrollview extension which supports infinite scrollview by assuming the same data structure and equal cell sizes.
    /// </summary>
    public interface IListView : IScrollView, IGrid {

        /// <summary>
        /// The total number of items to be displayed.
        /// </summary>
        int TotalItems { get; set; }

        /// <summary>
        /// Returns the position of the container at the starting point.
        /// </summary>
        Vector2 ContainerStartPos { get; }

        /// <summary>
        /// Returns the position of the container at the opposite of the starting point.
        /// </summary>
        Vector2 ContainerEndPos { get; }


        /// <summary>
        /// Initializes the listview with the specified event handlers.
        /// </summary>
        void Initialize(Func<IListItem> createItem, Action<IListItem> updateItem);

        /// <summary>
        /// Force-updates all visible cells in the view.
        /// </summary>
        void ForceUpdate();

        /// <summary>
        /// Re-calculates the number of cells to be pooled for the current rect transform size.
        /// </summary>
        void Recalibrate();

        /// <summary>
        /// Returns the first list item matching the specified predicate.
        /// </summary>
        IListItem FindItem(Func<IListItem, bool> predicate);
    }
}