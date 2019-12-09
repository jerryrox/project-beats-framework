using UnityEngine.UI;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object can be drawn using filling.
    /// </summary>
    public interface IHasFill {

        /// <summary>
        /// Amount of portion to fill the sprite.
        /// </summary>
        float FillAmount { get; set; }

        /// <summary>
        /// Whether the fill should be applied in an inverse way.
        /// </summary>
        bool FillInverse { get; set; }


        /// <summary>
        /// Sets fill mode to Radial 360.
        /// </summary>
        void SetRadial360Fill(Image.Origin360 origin);

        /// <summary>
        /// Sets fill mode to Radial 180.
        /// </summary>
        void SetRadial180Fill(Image.Origin180 origin);

        /// <summary>
        /// Sets fill mode to Radial 90.
        /// </summary>
        void SetRadial90Fill(Image.Origin90 origin);

        /// <summary>
        /// Sets fill mode to horizontal.
        /// </summary>
        void SetHorizontalFill(Image.OriginHorizontal origin);

        /// <summary>
        /// Sets fill mode to vertical.
        /// </summary>
        void SetVerticalFill(Image.OriginVertical origin);
    }
}