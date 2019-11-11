using System;

namespace PBFramework.Threading
{
    /// <summary>
    /// IProgress extension with event binding addon.
    /// </summary>
    public interface IEventProgress : ISimpleProgress {

        /// <summary>
        /// Events called when the process has been finished.
        /// </summary>
        event Action OnFinished;


        /// <summary>
        /// Invokes OnFinished event.
        /// </summary>
        void InvokeFinished();
    }
}