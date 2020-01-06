namespace PBFramework.UI.Navigations
{
    public abstract class UguiNavigationView : UguiPanel {

        public virtual HideActions HideAction => HideActions.Destroy;


        public virtual void ShowView()
        {
            if (OnPreShowView())
                OnPostShowView();
        }

        public virtual void HideView()
        {
            if(OnPreHideView())
                OnPostHideView();
        }

        /// <summary>
        /// Event called before this view is about to process view showing.
        /// Returns whether OnPostShowView can be called automatically after this method.
        /// </summary>
        protected virtual bool OnPreShowView()
        {
            Active = true;
            return true;
        }

        /// <summary>
        /// Event called after this view has finished showing the view.
        /// </summary>
        protected virtual void OnPostShowView() { }

        /// <summary>
        /// Event called before this view is about to process view hiding.
        /// Returns whether OnPostHideView can be called automatically after this method.
        /// </summary>
        protected virtual bool OnPreHideView() => true;

        /// <summary>
        /// Event called after this view has finished hiding the view.
        /// </summary>
        protected virtual void OnPostHideView() => Active = false;
    }
}