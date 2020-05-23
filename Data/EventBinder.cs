using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data
{
    /// <summary>
    /// Object which encapsulates temporary binding of events.
    /// </summary>
    public class EventBinder<T>
        where T : Delegate
    {
        private Action<T> bindEvent;
        private Action<T> unbindEvent;

        private T handler;


        /// <summary>
        /// Returns the event handler this responds to the target event.
        /// </summary>
        public T Handler => handler;

        /// <summary>
        /// Whether the event handler should be removed automatically after event emission.
        /// </summary>
        public bool IsOneTime { get; set; } = false;


        public EventBinder(Action<T> bindEvent, Action<T> unbindEvent)
        {
            if(bindEvent == null)
                throw new ArgumentNullException(nameof(bindEvent));
            if(unbindEvent == null)
                throw new ArgumentNullException(nameof(unbindEvent));

            this.bindEvent = bindEvent;
            this.unbindEvent = unbindEvent;
        }

        /// <summary>
        /// Binds the specified handler to target event.
        /// </summary>
        public void SetHandler(T handler)
        {
            RemoveHandler();

            if (handler == null)
                return;
            handler = InjectCustomHandler(handler);

            this.handler = handler;
            bindEvent.Invoke(handler);
        }

        /// <summary>
        /// Unbinds current handler from target event.
        /// </summary>
        public void RemoveHandler()
        {
            if (handler == null)
                return;
            unbindEvent.Invoke(handler);
            handler = null;
        }

        /// <summary>
        /// Injects an additional process in specified handler via delegate combination and returns the new handler.
        /// </summary>
        private T InjectCustomHandler(T handler)
        {
            Action injectedAction = () =>
            {
                // Remove the handler if it has been invoked once.
                if(IsOneTime)
                    RemoveHandler();
            };
            return (T)Delegate.Combine(handler, injectedAction);
        }
    }
}