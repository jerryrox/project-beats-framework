using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace PBFramework.Services
{
    /// <summary>
    /// Provides access to Unity-specific features which can't be accessed from classes other than MonoBehaviour type.
    /// </summary>
    public class UnityThreadService : MonoBehaviour {

        private static UnityThreadService I;
        private static MonoBehaviour MI;

        private int unityThreadId = -1;

        private Queue<DispatchedItem> dispatchedItems;


        /// <summary>
        /// Delegate for performing an action on the Unity thread.
        /// </summary>
        public delegate object DispatchedHandler();


        void Awake()
        {
            unityThreadId = Thread.CurrentThread.ManagedThreadId;
            dispatchedItems = new Queue<DispatchedItem>();
        }

        /// <summary>
        /// Returns the instance of this class.
        /// </summary>
        private static UnityThreadService Instance => I == null ? I = new GameObject("_UnityThreadService").AddComponent<UnityThreadService>() : I;

        /// <summary>
        /// Returns the instance of this class as MonoBehaviour type.
        /// </summary>
        private static MonoBehaviour MonoInstance => MI == null ? MI = Instance : MI;


        /// <summary>
        /// Manually initializes the service.
        /// </summary>
        public static void Initialize()
        {
            if(Instance != null) return;
        }

        /// <summary>
        /// Starts a new coroutine on the unity thread.
        /// </summary>
        public static new Coroutine StartCoroutine(IEnumerator enumerator) => MonoInstance.StartCoroutine(enumerator);

        /// <summary>
        /// Stops the specified coroutine if currently running.
        /// </summary>
        public static new void StopCoroutine(Coroutine coroutine) => MonoInstance.StopCoroutine(coroutine);

        /// <summary>
        /// Dispatches the specified action to be performed on the main thread.
        /// The caller thread will wait until the action is completed.
        /// </summary>
        public static object Dispatch(DispatchedHandler handler)
        {
            if(handler == null) throw new ArgumentNullException(nameof(handler));

            // If called from unity thread, just call the handler straight away.
            if (Thread.CurrentThread.ManagedThreadId == Instance.unityThreadId)
                return handler.Invoke();

            return Instance.DispatchInternal(new DispatchedItem(handler, true));
        }

        /// <summary>
        /// Dispatches the specified action to be performed on the main thread.
        /// The caller thread will not wait until the action is completed.
        /// </summary>
        public static void DispatchUnattended(DispatchedHandler handler)
        {
            if(handler == null) throw new ArgumentNullException(nameof(handler));

            // If called from unity thread, just call the handler straight away.
            if (Thread.CurrentThread.ManagedThreadId == Instance.unityThreadId)
            {
                handler.Invoke();
                return;
            }

            Instance.DispatchInternal(new DispatchedItem(handler, false));
        }

        /// <summary>
        /// Dispatches the item to Unity update process.
        /// </summary>
        private object DispatchInternal(DispatchedItem item)
        {
            lock (dispatchedItems)
            {
                dispatchedItems.Enqueue(item);
            }
            item.Standby();
            return item.ReturnData;
        }

        void Update()
        {
            lock (dispatchedItems)
            {
                while(dispatchedItems.Count > 0)
                {
                    dispatchedItems.Dequeue().Invoke();
                }
            }
        }


        /// <summary>
        /// Represents a single dispatched handler.
        /// </summary>
        private class DispatchedItem
        {
            private ManualResetEvent resetEvent;

            private DispatchedHandler handler;


            public bool IsAttended { get; set; }

            public object ReturnData { get; set; }


			public DispatchedItem(DispatchedHandler handler, bool isAttended)
            {
                this.handler = handler;
                this.IsAttended = isAttended;

                if(isAttended)
                    resetEvent = new ManualResetEvent(false);
			}

			/// <summary>
			/// Invokes the stored process.
			/// </summary>
			public void Invoke()
            {
                ReturnData = handler.Invoke();
                if(IsAttended)
                    resetEvent.Set();
			}

            /// <summary>
            /// Pauses the current thread.
            /// </summary>
            public void Standby()
            {
                if(IsAttended)
                    resetEvent.WaitOne();
            }
        }
    }
}