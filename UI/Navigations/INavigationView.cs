using System;
using PBFramework.Animations;

namespace PBFramework.UI.Navigations
{
    public interface INavigationView : IPanel, INavigationEvent {

        /// <summary>
        /// Event called when the view was requested to show.
        /// </summary>
        event Action OnShow;

        /// <summary>
        /// Event called when the view was requested to hide.
        /// </summary>
        event Action OnHide;


        /// <summary>
        /// Returns the type of action performed on hiding.
        /// </summary>
        HideActions HideAction { get; }

        /// <summary>
        /// Returns the animation played on view show.
        /// </summary>
        IAnime ShowAnime { get; }
        
        /// <summary>
        /// Returns the animation played on view hide.
        /// </summary>
        IAnime HideAnime { get; }
    }
}