using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Threading;
using UnityEngine.Networking;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Networking
{
    public class WebRequest : IWebRequest {

        public event Action OnFinished;
        public event Action<float> OnProgress;

        protected UnityWebRequest request;
        protected WebResponse response;

        protected WebLink link;
        protected int timeout;

        protected bool isDisposed = false;

        private Coroutine requestRoutine;
        private IReturnableProgress<IWebRequest> progressListener;

        private uint curRetryCount;
        private uint retryCount;


        public object Extra { get; set; }

        public uint RetryCount { get => retryCount; set => curRetryCount = retryCount = value; }

        public bool UseServerCaching { get; set; } = false;

        public bool IsAlive => request != null;

        public virtual bool IsDone => request != null && request.isDone;
        public bool IsFinished => IsDone;

        public float Progress
        {
            get
            {
                if(request == null) return 0;
                return request.downloadProgress > 0 ? request.downloadProgress : request.uploadProgress;
            }
        }

        public int Timeout
        {
            get => timeout;
            set
            {
                timeout = value;
                if(request != null)
                    request.timeout = value;
            }            
        }

        public virtual string Url => link.Url;

        public IWebResponse Response => response;

        public virtual object RawResult => this;


        public WebRequest(string url, int timeout = 60, int retryCount = 0)
        {
            this.link = new WebLink(url.GetUriEscaped());
            this.timeout = timeout;

            // Create response data
            this.response = new WebResponse(this);

            // Setup default event actions.
            OnFinished += () => progressListener?.InvokeFinished(this);
            OnProgress += (p) => progressListener?.Report(p);
        }

        public void Request(IReturnableProgress<IWebRequest> progress = null)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(WebRequest));
            if (request != null)
            {
                Logger.LogWarning("WebRequest.Request - There is already an on-going request!");
                return;
            }

            // Associate progress listener.
            progressListener = progress;
            if(progressListener != null)
                progressListener.Value = this;

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
            if(isDisposed) throw new ObjectDisposedException(nameof(WebRequest));
            if (request == null)
            {
                Logger.LogWarning("WebRequest.Abort - There is no request to abort!");
                return;
            }
            DisposeSoft();
        }

        public void Retry()
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(WebRequest));
            // Abort first if there is already a request.
            if(request != null)
                Abort();
            Request(progressListener);
        }

        public void Start() => Request();

        public void Revoke() => Abort();

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
        }

        /// <summary>
        /// Dispose this object hard so it can't be further used.
        /// </summary>
        protected virtual void DisposeHard()
        {
            if(isDisposed) return;
            isDisposed = true;
            DisposeSoft();

            if (response != null)
            {
                response.Dispose();
                response = null;
            }
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
                OnProgress?.Invoke(Progress);
                yield return null;
            }
            progressListener?.Report(1);

            // If request failed and there are auto retires remaining
            if (!response.IsSuccess && curRetryCount > 0)
            {
                // Retry.
                curRetryCount--;
                Retry();
            }
            else
            {
                // Fire event.
                OnFinished?.Invoke();
            }
        }
    }
}