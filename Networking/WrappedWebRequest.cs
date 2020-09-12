using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Threading;

namespace PBFramework.Networking
{
    /// <summary>
    /// Web request which wraps over another WebRequest with ITask as the base interface.
    /// </summary>
    public abstract class WrappedWebRequest<TRequest, TOutput> : ITask<TOutput>
        where TRequest : WebRequest
        where TOutput : class
    {
        /// <summary>
        /// Event called when the request has been finished.
        /// </summary>
        public event Action<TOutput> OnFinished;

        /// <summary>
        /// The output value from the wrapped request.
        /// </summary>
        public TOutput Output { get; private set; }

        public bool DidRun => request.IsAlive;

        public bool IsFinished => request.IsFinished;

        public float Progress => request.Progress;

        protected TRequest request;


        protected WrappedWebRequest(TRequest request)
        {
            this.request = request;
        }

        public void StartTask(TaskListener<TOutput> listener = null)
        {
            ListenToRequest(listener, () => listener?.SetFinished(Output));
            request.Request();
        }

        void ITask.StartTask(TaskListener listener)
        {
            ListenToRequest(listener, () => listener?.SetFinished());
            request.Request();
        }

        public void RevokeTask(bool dispose)
        {
            (request as ITask).RevokeTask(dispose);
        }

        /// <summary>
        /// Returns the output evaluated from the specified request.
        /// </summary>
        protected abstract TOutput GetOutput(TRequest request);

        /// <summary>
        /// Listens to events emitted from the wrapped request.
        /// </summary>
        private void ListenToRequest(TaskListener listener, Action onFinished)
        {
            request.OnFinished += (req) =>
            {
                Output = GetOutput(request);

                onFinished.Invoke();
                this.OnFinished?.Invoke(Output);
            };
            if(listener != null)
                request.OnProgress += listener.SetProgress;
        }
    }
}