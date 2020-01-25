using PBFramework.Graphics;

namespace PBFramework.UI.Navigations
{
    public class ScreenNavigator : Navigator, IScreenNavigator {
    
        public ScreenNavigator(IGraphicObject root) : base(root) {}

        protected override void ShowInternal(INavigationView view)
        {
            // Hide all other views.
            for (int i = views.Count - 1; i >= 0; i--)
            {
                HideInternal(views[i]);
            }
            base.ShowInternal(view);
        }
    }
}