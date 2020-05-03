namespace PBFramework.Animations
{
    /// <summary>
    /// Types of modes applied when animation is about to wrap.
    /// </summary>
    public enum WrapModeType {
    
        /// <summary>
        /// The animation will stay at its last time frame upon finishing.
        /// </summary>
        None,

        /// <summary>
        /// The animation will stop and return to the beginning of the time frame.
        /// </summary>
        Reset,

        /// <summary>
        /// The animation will loop from the start.
        /// </summary>
        Loop,
    }
}