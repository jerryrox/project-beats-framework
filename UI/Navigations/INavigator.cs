using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.UI.Navigations
{
    /// <summary>
    /// UI management unit which handles screen/overlay navigation.
    /// </summary>
    public interface INavigator<T>
        where T : INavigationView
    {
        /// <summary>
        /// Returns the view of specified type, if it has been cached.
        /// </summary>
        TView Get<TView>() where TView : T;

        /// <summary>
        /// Returns all views of specified type, if it has been cached.
        /// </summary>
        IEnumerable<TView> GetAll<TView>() where TView : T;

        /// <summary>
        /// Shows the view of specified type.
        /// </summary>
        TView Show<TView>() where TView : MonoBehaviour, T;

        /// <summary>
        /// Hides all cached views of specified type.
        /// </summary>
        void Hide<TView>() where TView : T;

        /// <summary>
        /// Hides the specified view.
        /// </summary>
        void Hide<TView>(TView view) where TView : T;
    }
}