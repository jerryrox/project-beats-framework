using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Graphics;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.UI.Navigations
{
    public class Navigator : INavigator
    {
        public event Action<INavigationView> OnShowView;

        public event Action<INavigationView> OnHideView;

        /// <summary>
        /// List of views currently showing or being cached.
        /// </summary>
        protected List<INavigationView> views = new List<INavigationView>();

        /// <summary>
        /// The container object which holds all the view objects.
        /// </summary>
        protected IGraphicObject root;


        public Navigator(IGraphicObject root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));

            this.root = root;
        }

        public T Get<T>()
            where T : INavigationView => views.OfType<T>().FirstOrDefault();

        public IEnumerable<T> GetAll<T>()
            where T : INavigationView => views.OfType<T>();

        public bool IsActive(Type type) => views.Any(v => v.Active && type.IsAssignableFrom(v.GetType()));

        public T Show<T>()
            where T : MonoBehaviour, INavigationView
        {
            var view = Get<T>();
            if (view == null)
            {
                view = CreateInternal<T>();
                views.Add(view);
            }

            ShowInternal(view);
            return view;
        }

        public void Hide<T>()
            where T : INavigationView
        {
            var view = Get<T>();
            if(view == null) return;

            HideInternal(view);
        }

        public void Hide<T>(T view)
            where T : INavigationView
        {
            if(view == null) return;

            HideInternal(view);
        }

        /// <summary>
        /// Internally handles view creation process.
        /// </summary>
        protected virtual T CreateInternal<T>()
            where T : MonoBehaviour, INavigationView
        {
            var view = root.CreateChild<T>(typeof(T).Name);
            var viewEvent = view as INavigationEvent;

            // Hook events to animations.
            var showAni = view.ShowAnime;
            var hideAni = view.HideAnime;
            if (showAni != null)
            {
                showAni.AddEvent(showAni.Duration, () =>
                {
                    if (viewEvent != null)
                        viewEvent.OnPostShow();
                });
            }
            if (hideAni != null)
            {
                hideAni.AddEvent(hideAni.Duration, () =>
                {
                    if (view != null)
                        DisposeInternal(view);
                });
            }
            return view;
        }

        /// <summary>
        /// Internally handles view showing process.
        /// </summary>
        protected virtual void ShowInternal(INavigationView view)
        {
            view.Active = true;

            OnShowView?.Invoke(view);

            view.OnPreShow();
            if (view.ShowAnime != null)
                view.ShowAnime.PlayFromStart();
            else
                view.OnPostShow();
        }

        /// <summary>
        /// Internally handles view hiding process.
        /// </summary>
        protected virtual void HideInternal(INavigationView view)
        {
            if(!view.Active)
                return;

            OnHideView?.Invoke(view);

            view.OnPreHide();
            if (view.HideAnime != null)
                view.HideAnime.PlayFromStart();
            else
                DisposeInternal(view);
        }

        /// <summary>
        /// Disposes the view based on its hide action type.
        /// </summary>
        protected virtual void DisposeInternal(INavigationView view)
        {
            view.OnPostHide();

            switch (view.HideAction)
            {
                case HideActions.Recycle:
                    view.Active = false;
                    break;
                case HideActions.Destroy:
                    views.Remove(view);
                    GameObject.Destroy(view.RawObject);
                    break;
                default:
                    Logger.LogWarning($"Navigator.DisposeInternal - Unsupported hide action type: {view.HideAction}");
                    break;
            }
        }
    }
}