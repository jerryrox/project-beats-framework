

namespace PBFramework.Graphics
{
    public static class NguiGraphicsHelper
    {
        public static Pivots NguiToGraphicsPivot(UIWidget.Pivot pivot)
        {
            return Pivots.TopLeft;
        }

        public static UIWidget.Pivot GraphicsToNguiPivot(Pivots pivot)
        {
            return UIWidget.Pivot.TopLeft;
        }
    }
}