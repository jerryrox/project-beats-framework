using PBFramework.Graphics;
using PBFramework.Animations;
using PBFramework.Dependencies;
using UnityEngine;

namespace PBFramework.UI.Navigations
{
    public abstract class UguiNavigationView : UguiPanel, INavigationView {

        public virtual HideActions HideAction => HideActions.Destroy;

        public IAnime ShowAnime { get; private set; }

        public IAnime HideAnime { get; private set; }


        [InitWithDependency]
        private void Init()
        {
            Anchor = Anchors.Fill;
            RawSize = Vector2.zero;

            // Create animations.
            ShowAnime = CreateShowAnime(Dependencies);
            HideAnime = CreateHideAnime(Dependencies);
        }

        /// <summary>
        /// Returns a new instance of the view show animation.
        /// A dependency container is passed since the super type wouldn't be initialized by this time.
        /// </summary>
        protected virtual IAnime CreateShowAnime(IDependencyContainer dependencies) => null;

        /// <summary>
        /// Returns a new instance of the view hide animation.
        /// A dependency container is passed since the super type wouldn't be initialized by this time.
        /// </summary>
        protected virtual IAnime CreateHideAnime(IDependencyContainer dependencies) => null;

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