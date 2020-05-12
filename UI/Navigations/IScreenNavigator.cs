using System;

namespace PBFramework.UI.Navigations
{
    public interface IScreenNavigator : INavigator {

        /// <summary>
        /// Event called when the screen is changed from previous to current view.
        /// Called after INavigator.OnShowView.
        /// First param = Current view
        /// Second param = Previous view
        /// </summary>
        event Action<INavigationView, INavigationView> OnScreenChange;

    
        /// <summary>
        /// The current screen being focused, if exists.
        /// </summary>
        INavigationView CurrentScreen { get; }

        /// <summary>
        /// Returns the previous screen before current, if exists.
        /// </summary>
        INavigationView PreviousScreen { get; }
    }
}