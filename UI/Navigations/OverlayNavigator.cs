using PBFramework.Graphics;

namespace PBFramework.UI.Navigations
{
    public class OverlayNavigator : Navigator<IOverlayView>, IOverlayNavigator {
    
        public OverlayNavigator(IGraphicObject root) : base(root) {}
    }
}