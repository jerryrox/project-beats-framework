using System;
using PBFramework.Threading;

namespace PBFramework.Networking
{
    /// <summary>
    /// Extension of WebRequest for use with Unity asset loading.
    /// </summary>
    public abstract class AssetRequest<T> : WebRequest, ITask<T>
        where T : UnityEngine.Object
    {
        /// <summary>
        /// Event called when the request has finished and an output was evaluated.
        /// </summary>
        public event Action<T> OnOutput;

        /// <summary>
        /// The output evaluated from the request.
        /// </summary>
        public T Output { get; protected set; }


        protected AssetRequest(string url, int timeout = 60, int retryCount = 1) :
            base(
                $"{(url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? "" : (url.StartsWith("file:", StringComparison.OrdinalIgnoreCase) ? "" : "file://"))}{url}",
                timeout: timeout,
                retryCount: retryCount
            )
        {
            base.OnFinished += (req) => OnOutput?.Invoke(Output);
        }

        void ITask<T>.StartTask(TaskListener<T> listener)
        {
            TaskListener<IWebRequest> newListener = null;
            if (listener != null)
            {
                listener.HasOwnProgress = false;
                newListener = listener.CreateSubListener<IWebRequest>();
                newListener.OnFinished += (req) => listener.SetFinished(Output);
            }
            Request(newListener);
        }

        protected override void DisposeSoft()
        {
            base.DisposeSoft();
            if (!IsDisposed)
                Output = null;
        }
    }
}