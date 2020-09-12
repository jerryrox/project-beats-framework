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

        protected TRequest request;


        protected WrappedWebRequest(TRequest request)
        {
            this.request = request;
        }

        public void StartTask(TaskListener<TOutput> listener)
        {
            request.Request(ConvertListener(listener, () => listener?.SetFinished(Output)));
        }

        public void StartTask(TaskListener listener)
        {
            request.Request(ConvertListener(listener, () => listener?.SetFinished()));
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
        /// Converts the specified listener to a compatible listener for the wrapped requester.
        /// </summary>
        private TaskListener<IWebRequest> ConvertListener(TaskListener listener, Action onFinished)
        {
            if(listener == null)
                return null;
            TaskListener<IWebRequest> newListener = listener.CreateSubListener<IWebRequest>();
            newListener.OnFinished += (req) =>
            {
                Output = GetOutput(request);
                
                onFinished?.Invoke();
                this.OnFinished?.Invoke(Output);
            };
            listener.HasOwnProgress = false;
            return newListener;
        }
    }
}