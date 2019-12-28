namespace PBFramework.Animations
{
    /// <summary>
    /// Types of modes applied when manually calling the IAnime Stop() method.
    /// </summary>
    public enum StopModes {
    
        /// <summary>
        /// The animation will stay where it was upon calling Stop().
        /// </summary>
        None,

        /// <summary>
        /// The animation will be reset to the beginning of the time frame.
        /// </summary>
        Reset,

        /// <summary>
        /// The animation will be reset to the end of the time frame.
        /// </summary>
        End
    }
}