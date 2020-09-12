using System;
using PBFramework.Threading;

namespace PBFramework.Networking
{
    /// <summary>
    /// Interface of an object which can make web requests to local or remote server.
    /// </summary>
    public interface IWebRequest : ITask<IWebRequest> {

        /// <summary>
        /// Event called when this request has finished.
        /// </summary>
        event Action<IWebRequest> OnFinished;

        /// <summary>
        /// Event called when this request has a new progress.
        /// </summary>
        event Action<float> OnProgress;


        /// <summary>
        /// An extra data that can be associated with this request.
        /// For example, you can assign an int value here to uniquely identify each request.
        /// </summary>
        object Extra { get; set; }

        /// <summary>
        /// Sets the number of automatic retries to make if current request fails.
        /// </summary>
        uint RetryCount { get; set; }

        /// <summary>
        /// Whether to use caching feature that is provided automatically from the server.
        /// Default: false
        /// </summary>
        bool UseServerCaching { get; set; }

        /// <summary>
        /// Whether there is currently a valid request which is either on-going or finished.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Returns whether this request has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Returns the current progress of the request.
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// The timeout time of the request in seconds.
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// The URL which the request is being made to.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Returns the response data of the last completed request.
        /// May return null if the current request is disposed or is not yet done.
        /// </summary>
        IWebResponse Response { get; }


        /// <summary>
        /// Makes the web request to remote or local server.
        /// </summary>
        void Request(TaskListener<IWebRequest> listener = null);

        /// <summary>
        /// Attempts to abort current request if on-going.
        /// </summary>
        void Abort();

        /// <summary>
        /// Retries the request.
        /// </summary>
        void Retry();
    }
}