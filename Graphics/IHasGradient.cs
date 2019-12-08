using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object supports gradient coloring.
    /// </summary>
    public interface IHasGradient : IHasColor {
    
        /// <summary>
        /// Whether gradient should be used.
        /// Default: false.
        /// </summary>
        bool UseGradient { get; set; }

        /// <summary>
        /// The color of the top gradient.
        /// </summary>
        Color TopGradient { get; set; }

        /// <summary>
        /// The color of the bottom gradient.
        /// </summary>
        Color BottomGradient { get; set; }
    }
}