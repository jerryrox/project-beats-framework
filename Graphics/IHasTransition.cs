using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a transition effect.
    /// </summary>
    public interface IHasTransition {
        
        /// <summary>
        /// Sets transition mode to none.
        /// (Default)
        /// </summary>
        void SetNoTransition();

        /// <summary>
        /// Sets transition mode to sprite swap.
        /// </summary>
        void SetSpriteSwapTransition(Sprite highlight, Sprite selected, Sprite pressed, Sprite disabled);

        /// <summary>
        /// Sets transition mode to color tint.
        /// </summary>
        void SetColorTintTransition(Color normal, Color highlight, Color selected, Color pressed, Color disabled, float duration);

        /// <summary>
        /// Sets transition mode to color tint.
        /// Selected, pressed, disabled colors are adjusted automatically.
        /// </summary>
        void SetColorTintTransition(Color normal, float duration);
    }
}