using System;
using System.Collections;
using UnityEngine;
using PBFramework.Threading;
using PBFramework.Threading.Futures;
using UnityEngine.Networking;

namespace PBFramework.Networking
{
    public abstract class WebRequest<T> : Future<T>, IWebRequest {

        protected UnityWebRequest request;
        protected WebResponse response;

        protected WebLink link;
        protected int timeout;

        private Coroutine requestRoutine;

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


        protected WebRequest(string url, int timeout = 60, int retryCount = 0)
        {
            this.link = new WebLink(url.GetUriEscaped());
            this.timeout = timeout;

            // Create response data
            this.response = new WebResponse(this);

            SetHandler((_) => Request());
        }

        public IFuture Request()
        {
            AssertNotDisposed();
            if (request != null)
                throw new Exception("There is already an on-going request!");

            RequestInternal();
            return this;
        }

        public void Abort() => Dispose();

        public override void Dispose()
        {
            DisposeRequest();
            base.Dispose();
        }

        /// <summary>
        /// Evalutes the web response data.
        /// </summary>
        protected abstract T EvaluateResponse();

        /// <summary>
        /// Creates a new UnityWebRequest instance for requesting.
        /// </summary>
        protected virtual UnityWebRequest CreateRequest(string url) => UnityWebRequest.Get(url);

        /// <summary>
        /// Disposes request-related process and variables.
        /// </summary>
        private void DisposeRequest()
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
            if (response != null)
            {
                response.Dispose();
                response = null;
            }
        }

        /// <summary>
        /// Handles the actual logics for making a new request.
        /// </summary>
        private void RequestInternal()
        {
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

        /// <summary>
        /// Retries the web request.
        /// </summary>
        private void Retry()
        {
            DisposeRequest();
            RequestInternal();
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

            // If request failed and there are auto retries remaining
            if (!response.IsRequestSuccess && curRetryCount > 0)
            {
                // Retry.
                curRetryCount--;
                Retry();
            }
            else if (!response.IsRequestSuccess)
                SetFail(new Exception(response.ErrorMessage));
            else
                SetComplete(EvaluateResponse());
        }
    }

    public class WebRequest : WebRequest<WebRequest>
    {
        public WebRequest(string url, int timeout = 60, int retryCount = 0) :
            base(url, timeout, retryCount)
        {
        }

        protected override WebRequest EvaluateResponse() => this;
    }

}