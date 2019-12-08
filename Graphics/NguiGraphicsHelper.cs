using System;

namespace PBFramework.Graphics
{
    public static class NguiGraphicsHelper
    {
        public static Pivots NguiToGraphicsPivot(UIWidget.Pivot pivot)
        {
            return (Pivots)pivot;
        }

        public static UIWidget.Pivot GraphicsToNguiPivot(Pivots pivot)
        {
            return (UIWidget.Pivot)pivot;
        }

        public static SizeWrapModes NguiToGraphicsOverflow(UILabel.Overflow overflow)
        {
            switch (overflow)
            {
                case UILabel.Overflow.ResizeFreely: return SizeWrapModes.Free;
                case UILabel.Overflow.ClampContent: return SizeWrapModes.Clamp;
                case UILabel.Overflow.ShrinkContent: return SizeWrapModes.Shrink;
                case UILabel.Overflow.ResizeHeight: return SizeWrapModes.ResizeHeight;
            }
            throw new ArgumentException($"Unknown overflow type: {overflow}");
        }

        public static UILabel.Overflow GraphicsToNguiOverflow(SizeWrapModes mode)
        {
            switch (mode)
            {
                case SizeWrapModes.Free: return UILabel.Overflow.ResizeFreely;
                case SizeWrapModes.Clamp: return UILabel.Overflow.ClampContent;
                case SizeWrapModes.Shrink: return UILabel.Overflow.ShrinkContent;
                case SizeWrapModes.ResizeHeight: return UILabel.Overflow.ResizeHeight;
            }
            throw new ArgumentException($"Unknown wrap mode: {mode}");
        }

        public static Alignments NguiToGraphicsAlignment(NGUIText.Alignment alignment)
        {
            switch (alignment)
            {
                case NGUIText.Alignment.Left: return Alignments.Left;
                case NGUIText.Alignment.Center: return Alignments.Center;
                case NGUIText.Alignment.Right: return Alignments.Right;
            }
            throw new ArgumentException($"Unknown alignment mode: {alignment}");
        }

        public static NGUIText.Alignment GraphicsToNguiAlignment(Alignments alignment)
        {
            switch (alignment)
            {
                case Alignments.Left: return NGUIText.Alignment.Left;
                case Alignments.Center: return NGUIText.Alignment.Center;
                case Alignments.Right: return NGUIText.Alignment.Right;
            }
            throw new ArgumentException($"Unknown wrap mode: {alignment}");
        }
    }
}