using PBFramework.Graphics;

namespace PBFramework.UI.Navigations
{
    public class ScreenNavigator : Navigator, IScreenNavigator {

        public INavigationView CurrentScreen { get; private set; }


        public ScreenNavigator(IGraphicObject root) : base(root) {}

        protected override void ShowInternal(INavigationView view, bool checkActive)
        {
            // Hide all other views.
            for (int i = views.Count - 1; i >= 0; i--)
            {
                if(views[i] != view)
                    HideInternal(views[i]);
            }

            CurrentScreen = view;
            base.ShowInternal(view, checkActive);
        }

        protected override void HideInternal(INavigationView view)
        {
            if(view == CurrentScreen)
                CurrentScreen = null;
                
            base.HideInternal(view);
        }
    }
}