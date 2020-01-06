using PBFramework.Graphics;

namespace PBFramework.UI.Navigations
{
    public class ScreenNavigator : Navigator<IScreenView>, IScreenNavigator {
    
        public ScreenNavigator(IGraphicObject root) : base(root) {}

        protected override void OnPreShowView(IScreenView view)
        {
            // Hide all other views except the view about to be shown.
            for (int i = views.Count - 1; i >= 0; i--)
            {
                if(views[i] == view) continue;
                if (views[i].HideAction == HideActions.Destroy)
                {
                    OnViewDestroying(views[i]);
                    HideInternal(views[i]);
                    views.RemoveAt(i);
                }
                else
                {
                    views[i].HideView();
                }
            }
        }
    }
}