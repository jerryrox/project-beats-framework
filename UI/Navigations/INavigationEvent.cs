namespace PBFramework.UI.Navigations
{
    public interface INavigationEvent {

        /// <summary>
        /// Event called from navigator when the view is about to be shown.
        /// </summary>
        void OnPreShow();

        /// <summary>
        /// Event called from navigator when the view is fully shown after animation.
        /// </summary>
        void OnPostShow();

        /// <summary>
        /// Event called from navigator when the view is about to be hidden.
        /// </summary>
        void OnPreHide();

        /// <summary>
        /// Event called from navigator when the view is fully hidden after animation.
        /// </summary>
        void OnPostHide();
    }
}