using System;
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
        /// Event called on view show.
        /// </summary>
        event Action<INavigationView> OnShowView;

        /// <summary>
        /// Event called on view hide.
        /// </summary>
        event Action<INavigationView> OnHideView;


        /// <summary>
        /// Returns the view of specified type, if it has been cached.
        /// </summary>
        T Get<T>() where T : INavigationView;

        /// <summary>
        /// Returns all views of specified type, if it has been cached.
        /// </summary>
        IEnumerable<T> GetAll<T>() where T : INavigationView;

        /// <summary>
        /// Returns whether the view of specified type is active.
        /// </summary>
        bool IsActive(Type type);

        /// <summary>
        /// Returns whether the view of specified type is active and is not currently hiding.
        /// </summary>
        bool IsShowing(Type type);

        /// <summary>
        /// Creates the view of specified type if not already created, and hides it immediately.
        /// </summary>
        T CreateHidden<T>() where T : MonoBehaviour, INavigationView;

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