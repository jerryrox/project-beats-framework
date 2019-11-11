using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PBFramework.Services;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Networking
{
    public class WebRequest : IWebRequest {

        /// <summary>
        /// Event called when a request has been finished.
        /// </summary>
        public event Action<IWebRequest> OnFinished;

        protected UnityWebRequest request;
        protected WebResponse response;

        protected string url;
        protected int timeout;

        protected bool isDisposed = false;

        private Coroutine requestRoutine;
        private IProgress<float> progressListener;

        private uint curRetryCount;
        private uint retryCount;


        public object Extra { get; set; }

        public uint RetryCount { get => retryCount; set => curRetryCount = retryCount = value; }

        public bool IsAlive => request != null;

        public virtual bool IsDone => request != null && request.isDone;

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

        public string Url => url;

        public IWebResponse Response => response;


        public WebRequest(string url, int timeout = 60, int retryCount = 0)
        {
            this.url = url.GetUriEscaped();
            this.timeout = timeout;
        }

        public void Request(IProgress<float> progress = null)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(WebRequest));
            if (request != null)
            {
                Logger.LogWarning("WebRequest.Request - There is already an on-going request!");
                return;
            }

            this.progressListener = progress;

            // Dispose last request
            DisposeSoft();

            // Prepare request
            request = CreateRequest(url);
            request.timeout = timeout;

            // Start polling request completion on coroutine.
            requestRoutine = UnityThreadService.StartCoroutine(RequestRoutine());
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
                UnityThreadService.StopCoroutine(requestRoutine);
                requestRoutine = null;
            }
            if (request != null)
            {
                request.Abort();
                request.Dispose();
                request = null;
            }
            if (response != null)
            {
                response.Dispose();
                response = null;
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
        }

        /// <summary>
        /// Handles polling of request end signal.
        /// </summary>
        private IEnumerator RequestRoutine()
        {
            // Send request
            request.SendWebRequest();

            // Create response data
            response = new WebResponse(this, request);

            // Polling till finished.
            while (request != null && !request.isDone && !request.isNetworkError)
            {
                progressListener?.Report(Progress);
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
                OnFinished?.Invoke(this);
            }
        }
    }
}