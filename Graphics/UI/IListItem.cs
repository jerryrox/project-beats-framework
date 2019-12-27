namespace PBFramework.Graphics.UI
{
    /// <summary>
    /// Indicates an item which can be scrolled infinitely via IListView.
    /// </summary>
    public interface IListItem : IGraphicObject {
    
        /// <summary>
        /// Current index of the item relative to the list view children.
        /// Normally, this shouldn't be modified by any other class but the listview.
        /// </summary>
        int ItemIndex { get; set; }
    }
}