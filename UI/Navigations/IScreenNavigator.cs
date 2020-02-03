namespace PBFramework.UI.Navigations
{
    public interface IScreenNavigator : INavigator {
    
        /// <summary>
        /// The current screen being focused, if exists.
        /// </summary>
        INavigationView CurrentScreen { get; }
    }
}