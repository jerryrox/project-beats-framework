using PBFramework.Animations;
using PBFramework.Dependencies;

namespace PBFramework.UI.Navigations
{
    public abstract class UguiNavigationView : UguiPanel, INavigationView {

        public virtual HideActions HideAction => HideActions.Destroy;

        public IAnime ShowAnime { get; private set; }

        public IAnime HideAnime { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            // Create animations.
            ShowAnime = CreateShowAnime();
            HideAnime = CreateHideAnime();
        }

        /// <summary>
        /// Returns a new instance of the view show animation.
        /// </summary>
        protected virtual IAnime CreateShowAnime() => null;

        /// <summary>
        /// Returns a new instance of the view hide animation.
        /// </summary>
        protected virtual IAnime CreateHideAnime() => null;

        /// <summary>
        /// Handles pre-show event called from the navigator.
        /// </summary>
        protected virtual void OnPreShow() {}
        void INavigationEvent.OnPreShow() => OnPreShow();

        /// <summary>
        /// Handles post-show event called from the navigator.
        /// </summary>
        protected virtual void OnPostShow() { }
        void INavigationEvent.OnPostShow() => OnPostShow();

        /// <summary>
        /// Handles pre-hide event called from the navigator.
        /// </summary>
        protected virtual void OnPreHide() { }
        void INavigationEvent.OnPreHide() => OnPreHide();

        /// <summary>
        /// Handles post-hide event called from the navigator.
        /// </summary>
        protected virtual void OnPostHide() { }
        void INavigationEvent.OnPostHide() => OnPostHide();
    }
}