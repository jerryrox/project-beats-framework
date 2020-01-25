using PBFramework.Animations;

namespace PBFramework.UI.Navigations
{
    public interface INavigationView : IPanel, INavigationEvent {

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