using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Indicates that the object has a rect.
    /// </summary>
    public interface IHasOffset {

        /// <summary>
        /// The size offset of the object.
        /// </summary>
        Offset Offset { get; set; }
    }

    public static class HasOffsetExtensions
    {
        public static void SetOffsetLeft(this IHasOffset offset, float left)
        {
            var o = offset.Offset;
            o.Left = left;
            offset.Offset = o;
        }

        public static void SetOffsetTop(this IHasOffset offset, float top)
        {
            var o = offset.Offset;
            o.Top = top;
            offset.Offset = o;
        }

        public static void SetOffsetRight(this IHasOffset offset, float right)
        {
            var o = offset.Offset;
            o.Right = right;
            offset.Offset = o;
        }

        public static void SetOffsetBottom(this IHasOffset offset, float bottom)
        {
            var o = offset.Offset;
            o.Bottom = bottom;
            offset.Offset = o;
        }

        public static void SetOffsetVertical(this IHasOffset offset, float vertical)
        {
            var o = offset.Offset;
            o.Vertical = vertical;
            offset.Offset = o;
        }

        public static void SetOffsetVertical(this IHasOffset offset, float top, float bottom)
        {
            var o = offset.Offset;
            o.Top = top;
            o.Bottom = bottom;
            offset.Offset = o;
        }

        public static void SetOffsetHorizontal(this IHasOffset offset, float horizontal)
        {
            var o = offset.Offset;
            o.Horizontal = horizontal;
            offset.Offset = o;
        }

        public static void SetOffsetHorizontal(this IHasOffset offset, float left, float right)
        {
            var o = offset.Offset;
            o.Left = left;
            o.Right = right;
            offset.Offset = o;
        }
    }
}