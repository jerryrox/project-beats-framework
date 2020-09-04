using System;
using System.Collections;
using UnityEngine;
using PBFramework.Data.Bindables;
using PBFramework.Threading;
using UnityEngine.Networking;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Networking
{
    public class WebRequest : IWebRequest {

        protected UnityWebRequest request;
        protected WebResponse response;

        protected WebLink link;
        protected int timeout;

        private BindableFloat progress = new BindableFloat(0f)
        {
            TriggerWhenDifferent = true
        };
        private BindableBool isDisposed = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };
        private BindableBool isCompleted = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };
        private Bindable<Exception> error = new Bindable<Exception>(null);

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

        public IReadOnlyBindable<float> Progress => progress;
        public IReadOnlyBindable<bool> IsDisposed => isDisposed;
        public IReadOnlyBindable<bool> IsCompleted => isCompleted;
        public IReadOnlyBindable<Exception> Error => error;
        public bool DidRun => IsAlive;
        public bool IsThreadSafe
        {
            get => true;
            set => throw new NotSupportedException();
        }


        public WebRequest(string url, int timeout = 60, int retryCount = 0)
        {
            this.link = new WebLink(url.GetUriEscaped());
            this.timeout = timeout;
            this.RetryCount = (uint)Mathf.Clamp(retryCount, 0, retryCount);

            // Create response data
            this.response = new WebResponse(this);

            // Setup default event actions.
            isCompleted.OnNewValue += (completed) =>
            {
                if (completed)
                    listener?.SetFinished(this);
            };
            progress.OnNewValue += (p) => listener?.SetProgress(p);
        }

        public void Request(TaskListener<IWebRequest> listener = null)
        {
            if(isDisposed.Value) throw new ObjectDisposedException(nameof(WebRequest));
            if (request != null)
            {
                Logger.LogWarning("WebRequest.Request - There is already an on-going request!");
                return;
            }

            // Associate progress listener.
            this.listener = listener;
            if(this.listener != null)
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
            if(isDisposed.Value) throw new ObjectDisposedException(nameof(WebRequest));
            if (request == null)
            {
                Logger.LogWarning("WebRequest.Abort - There is no request to abort!");
                return;
            }
            DisposeSoft();
        }

        public void Retry()
        {
            if(isDisposed.Value) throw new ObjectDisposedException(nameof(WebRequest));
            // Abort first if there is already a request.
            if(request != null)
                Abort();
            Request(listener);
        }

        public void Start() => Request();

        public void Dispose() => DisposeHard();

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
            if (!isDisposed.Value)
            {
                progress.Value = 0f;
                error.Value = null;
                isCompleted.Value = false;
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
            if(isDisposed.Value) return;
            isDisposed.Value = true;
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
                float p = request.downloadProgress > 0 ? request.downloadProgress : request.uploadProgress;
                progress.Value = p;
                yield return null;
            }
            progress.Value = 1f;

            // If request failed and there are auto retires remaining
            if (!response.IsRequestSuccess && curRetryCount > 0)
            {
                // Retry.
                curRetryCount--;
                Retry();
            }
            else
            {
                if (!response.IsRequestSuccess)
                    error.Value = new Exception(response.ErrorMessage);
                else
                    EvaluateResponse();

                isCompleted.Value = true;
            }
        }
    }
}