using System;
using PBFramework.Data.Bindables;

namespace PBFramework.UI.Navigations
{
    public interface IScreenNavigator : INavigator {

        /// <summary>
        /// Returns the current screen being focused, if exists.
        /// </summary>
        IReadOnlyBindable<INavigationView> CurrentScreen { get; }

        /// <summary>
        /// Returns the previous screen before current, if exists.
        /// </summary>
        INavigationView PreviousScreen { get; }
    }
}