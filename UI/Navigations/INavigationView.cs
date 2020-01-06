namespace PBFramework.UI.Navigations
{
    public interface INavigationView : IPanel {

        /// <summary>
        /// Returns the type of action performed on hiding.
        /// </summary>
        HideActions HideAction { get; }


        /// <summary>
        /// Performs showing process of the view.
        /// </summary>
        void ShowView();

        /// <summary>
        /// Performs hiding process of the view.
        /// </summary>
        void HideView();
    }
}