using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Graphics;

namespace PBFramework.UI.Navigations
{
    public abstract class Navigator<T> : INavigator<T>
        where T : INavigationView
    {
        /// <summary>
        /// List of views currently showing or being cached.
        /// </summary>
        protected List<T> views = new List<T>();

        /// <summary>
        /// The container object which holds all the view objects.
        /// </summary>
        protected IGraphicObject root;


        public Navigator(IGraphicObject root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));

            this.root = root;
        }

        public TView Get<TView>()
            where TView : T => views.OfType<TView>().FirstOrDefault();

        public IEnumerable<TView> GetAll<TView>()
            where TView : T => views.OfType<TView>();

        public TView Show<TView>()
            where TView : MonoBehaviour, T
        {
            var view = Get<TView>();
            if (view != null)
            {
                ShowInternal(view);
                return view;
            }

            view = root.CreateChild<TView>();
            OnViewCreated(view);
            ShowInternal(view);
            views.Add(view);
            return view;
        }

        public void Hide<TView>()
            where TView : T
        {
            for (int i = views.Count - 1; i >= 0; i--)
            {
                if (views[i] is TView)
                {
                    if (views[i].HideAction == HideActions.Destroy)
                    {
                        OnViewDestroying(views[i]);
                        HideInternal(views[i]);
                        views.RemoveAt(i);
                    }
                    else
                    {
                        HideInternal(views[i]);
                    }
                }
            }
        }

        public void Hide<TView>(TView view)
            where TView : T
        {
            if (view == null) return;

            if (view.HideAction == HideActions.Destroy)
            {
                OnViewDestroying(view);
                HideInternal(view);
                views.Remove(view);
            }
            else
            {
                HideInternal(view);
            }
        }

        /// <summary>
        /// Event called when the specified view is newly instantiated.
        /// </summary>
        protected virtual void OnViewCreated(T view) {}

        /// <summary>
        /// Event called when the specified view is about to be destroyed.
        /// </summary>
        protected virtual void OnViewDestroying(T view) {}

        /// <summary>
        /// Event called when the specified view is about to show.
        /// </summary>
        protected virtual void OnPreShowView(T view) {}

        /// <summary>
        /// Event called when the specified view has been shown.
        /// </summary>
        protected virtual void OnPostShowView(T view) {}

        /// <summary>
        /// Event called when the specified view is about to hide.
        /// </summary>
        protected virtual void OnPreHideView(T view) {}

        /// <summary>
        /// Event called when the specified view has been hidden.
        /// </summary>
        protected virtual void OnPostHideView(T view) {}

        /// <summary>
        /// Handles showing of the specified view with events.
        /// </summary>
        protected void ShowInternal(T view)
        {
            OnPreShowView(view);
            view.ShowView();
            OnPostShowView(view);
        }

        /// <summary>
        /// Handles hiding of the specified view with events.
        /// </summary>
        protected void HideInternal(T view)
        {
            OnPreHideView(view);
            view.HideView();
            OnPostHideView(view);
        }
    }
}