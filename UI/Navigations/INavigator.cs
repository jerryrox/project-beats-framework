using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.UI.Navigations
{
    /// <summary>
    /// UI management unit which handles a general view navigation.
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Returns the view of specified type, if it has been cached.
        /// </summary>
        T Get<T>() where T : INavigationView;

        /// <summary>
        /// Returns all views of specified type, if it has been cached.
        /// </summary>
        IEnumerable<T> GetAll<T>() where T : INavigationView;

        /// <summary>
        /// Shows the view of specified type.
        /// </summary>
        T Show<T>() where T : MonoBehaviour, INavigationView;

        /// <summary>
        /// Hides the view of specified type.
        /// </summary>
        void Hide<T>() where T : INavigationView;

        /// <summary>
        /// Hides the specified view.
        /// </summary>
        void Hide<T>(T view) where T : INavigationView;
    }
}