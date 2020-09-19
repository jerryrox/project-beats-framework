using System;
using System.Collections;
using UnityEngine;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using UnityEngine.Networking;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Networking
{
    public class WebRequest : IWebRequest
    {
        public event Action<IWebRequest> OnFinished;

        public event Action<float> OnProgress;

        protected UnityWebRequest request;
        protected WebResponse response;

        protected WebLink link;
        protected int timeout;

        private Coroutine requestRoutine;
        private TaskListener<IWebRequest> listener;

        private uint curRetryCount;
        private uint retryCount;


        public object Extra { get; set; }

        public uint RetryCount
        {
            get => retryCount;
            set => curRetryCount = retryCount = value;
        }

        public bool UseServerCaching { get; set; } = false;

        public bool IsAlive => request != null;

        public bool IsFinished { get; private set; }

        public bool IsDisposed { get; private set; }

        public float Progress { get; private set; }

        public int Timeout
        {
            get => timeout;
            set
            {
                timeout = value;
                if (request != null)
                    request.timeout = value;
            }
        }

        public virtual string Url => link.Url;

        public IWebResponse Response => response;

        bool ITask.DidRun => IsAlive;


        public WebRequest(string url, int timeout = 60, int retryCount = 0)
        {
            this.link = new WebLink(url.GetUriEscaped());
            this.timeout = timeout;
            this.RetryCount = (uint)Mathf.Clamp(retryCount, 0, retryCount);

            // Create response data
            this.response = new WebResponse(this);

            // Setup default event actions.
            OnFinished += (request) => listener?.SetFinished(this);
            OnProgress += (p) => listener?.SetProgress(p);
        }

        public void Request(TaskListener<IWebRequest> listener = null)
        {
            AssertNotDisposed();
            if (request != null)
            {
                Logger.LogWarning("WebRequest.Request - There is already an on-going request!");
                return;
            }

            // Associate progress listener.
            this.listener = listener;
            if (this.listener != null)
                this.listener.SetValue(this);

            // Dispose last request
            DisposeSoft();

            // Prepare request
            request = CreateRequest(Url);
            request.timeout = timeout;
            request.useHttpContinue = false;

            // Use caching?
            if (!UseServerCaching)
            {
                request.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
                request.SetRequestHeader("Pragma", "no-cache");
            }

            // Assign requester object on response.
            response.Request = request;

            // Start polling request completion on coroutine.
            requestRoutine = UnityThread.StartCoroutine(RequestRoutine());
        }

        public void Abort()
        {
            AssertNotDisposed();
            if (request == null)
            {
                Logger.LogWarning("WebRequest.Abort - There is no request to abort!");
                return;
            }
            DisposeSoft();
        }

        public void Retry()
        {
            AssertNotDisposed();
            // Abort first if there is already a request.
            if (request != null)
                Abort();
            Request(listener);
        }

        void ITask<IWebRequest>.StartTask(TaskListener<IWebRequest> listener) => Request(listener);
        void ITask.StartTask(TaskListener listener)
        {
            TaskListener<IWebRequest> newListener = null;
            if (listener != null)
            {
                listener.HasOwnProgress = false;
                newListener = listener.CreateSubListener<IWebRequest>();
                newListener.OnFinished += (req) => listener.SetFinished();
            }
            Request(newListener);
        }
        void ITask.RevokeTask(bool dispose)
        {
            if (dispose)
                DisposeHard();
            else
                Abort();
        }

        /// <summary>
        /// Evalutes web response data, if required by the subtype.
        /// </summary>
        protected virtual void EvaluateResponse() { }

        /// <summary>
        /// Creates a new UnityWebRequest instance for requesting.
        /// </summary>
        protected virtual UnityWebRequest CreateRequest(string url) => UnityWebRequest.Get(url);

        /// <summary>
        /// Disposes this object softly so it can be further reused.
        /// </summary>
        protected virtual void DisposeSoft()
        {
            if (requestRoutine != null)
            {
                UnityThread.StopCoroutine(requestRoutine);
                requestRoutine = null;
            }
            if (request != null)
            {
                request.Abort();
                request.Dispose();
                request = null;
            }
            // Dispose the response, but do not remove the reference to it.
            if (response != null)
            {
                response.Dispose();
            }
            if (!IsDisposed)
            {
                Progress = 0f;
                IsFinished = false;
            }
        }

        /// <summary>
        /// Dispose this object hard so it can't be further used.
        /// </summary>
        protected virtual void DisposeHard()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            DisposeSoft();

            if (response != null)
            {
                response.Dispose();
                response = null;
            }
        }

        /// <summary>
        /// Flags the request as finished state, while invoking OnFinished event.
        /// </summary>
        protected virtual void SetFinished()
        {
            IsFinished = true;
            OnFinished?.Invoke(this);
        }

        /// <summary>
        /// Handles polling of request end signal.
        /// </summary>
        private IEnumerator RequestRoutine()
        {
            // Send request
            request.SendWebRequest();

            // Polling till finished.
            while (request != null && !request.isDone && !request.isNetworkError)
            {
                float p = request.downloadProgress > 0 ? request.downloadProgress : request.uploadProgress;
                SetProgress(p);
                yield return null;
            }
            SetProgress(1f);

            // If request failed and there are auto retires remaining
            if (!response.IsRequestSuccess && curRetryCount > 0)
            {
                // Retry.
                curRetryCount--;
                Retry();
            }
            else
            {
                if (response.IsRequestSuccess)
                    EvaluateResponse();

                SetFinished();
            }
        }

        /// <summary>
        /// Sets the current progress of the request, while invoking OnProgress event.
        /// </summary>
        private void SetProgress(float progress)
        {
            this.Progress = progress;
            OnProgress?.Invoke(progress);
        }

        /// <summary>
        /// Asserts that this object is not disposed.
        /// </summary>
        private void AssertNotDisposed()
        {
            if(IsDisposed)
                throw new ObjectDisposedException(nameof(WebRequest));
        }
    }
}