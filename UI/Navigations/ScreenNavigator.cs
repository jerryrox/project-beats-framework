using System;
using PBFramework.Data.Bindables;
using PBFramework.Graphics;

namespace PBFramework.UI.Navigations
{
    public class ScreenNavigator : Navigator, IScreenNavigator {

        private Bindable<INavigationView> curScreen = new Bindable<INavigationView>(null);


        public IReadOnlyBindable<INavigationView> CurrentScreen => curScreen;

        public INavigationView PreviousScreen { get; private set; }


        public ScreenNavigator(IGraphicObject root) : base(root) {}

        protected override void ShowInternal(INavigationView view, bool checkActive)
        {
            // Hide all other views.
            for (int i = views.Count - 1; i >= 0; i--)
            {
                if(views[i] != view)
                    HideInternal(views[i]);
            }

            base.ShowInternal(view, checkActive);
            curScreen.Value = view;
        }

        protected override void HideInternal(INavigationView view)
        {
            if (view == CurrentScreen)
            {
                PreviousScreen = curScreen.Value;
                curScreen.Value = null;
            }

            base.HideInternal(view);
        }
    }
}