namespace PBFramework.Graphics.UI.Elements
{
    /// <summary>
    /// Types of rendering modes a panel may exhibit.
    /// </summary>
    public enum PanelModes {
    
        /// <summary>
        /// No sizing affects panel.
        /// No clipping occurs.
        /// </summary>
        NoConstraints = 0,

        /// <summary>
        /// Sizing affects panel.
        /// No clipping occurs.
        /// </summary>
        NoClip,

        /// <summary>
        /// Sizing affects panel.
        /// Clipping occurs.
        /// </summary>
        Clip,
    }
}