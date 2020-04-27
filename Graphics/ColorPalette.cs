using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Color container which provides extra features over plain Color structure.
    /// </summary>
    public class ColorPalette {

        /// <summary>
        /// The base color of the palette.
        /// </summary>
        public Color Base { get; private set; }


        public ColorPalette(Color baseColor)
        {
            Base = baseColor;
        }

        /// <summary>
        /// Returns base color lightened by specified factor.
        /// </summary>
        public Color Lighten(float factor)
        {
            Color color = Base;
            color.r = Mathf.Lerp(color.r, 1f, factor);
            color.g = Mathf.Lerp(color.g, 1f, factor);
            color.b = Mathf.Lerp(color.b, 1f, factor);
            return color;
        }

        /// <summary>
        /// Returns base color darkened by specified factor.
        /// </summary>
        public Color Darken(float factor)
        {
            Color color = Base;
            color.r = Mathf.Lerp(color.r, 0f, factor);
            color.g = Mathf.Lerp(color.g, 0f, factor);
            color.b = Mathf.Lerp(color.b, 0f, factor);
            return color;
        }

        /// <summary>
        /// Returns base color scaled down to gray by specified factor.
        /// </summary>
        public Color Weaken(float factor)
        {
            Color color = Base;
            color.r = Mathf.Lerp(color.r, 0.5f, factor);
            color.g = Mathf.Lerp(color.g, 0.5f, factor);
            color.b = Mathf.Lerp(color.b, 0.5f, factor);
            return color;
        }

        /// <summary>
        /// Returns base color scaled out from gray by specified factor.
        /// </summary>
        public Color Strengthen(float factor)
        {
            Color color = Base;
            color.r = Mathf.Lerp(color.r, color.r > 0.5f ? 1f : 0f, factor);
            color.g = Mathf.Lerp(color.g, color.g > 0.5f ? 1f : 0f, factor);
            color.b = Mathf.Lerp(color.b, color.b > 0.5f ? 1f : 0f, factor);
            return color;
        }

        /// <summary>
        /// By default, the color palette can be converted directly to the base color impliticly.
        /// </summary>
        public static implicit operator Color(ColorPalette palette)
        {
            return palette.Base;
        }
    }
}