using System;
using System.Collections;
using UnityEngine;

namespace PBFramework.Threading
{
    public class AsyncOperationTask : ManualTask {

        private Func<AsyncOperation> runner;
        private AsyncOperation curOperation;


        public AsyncOperationTask(Func<AsyncOperation> runner)
        {
            this.runner = runner;
        }

        /// <summary>
        /// Starts running the async operation.
        /// </summary>
        private void StartRunner()
        {
            curOperation = runner?.Invoke();
            if (curOperation == null)
            {
                SetProgress(1f);
                SetFinished();
                return;
            }

            UnityThread.StartCoroutine(PollProgress());
            curOperation.completed += OnOperationComplete;
        }

        /// <summary>
        /// Event called when the current async operation has finished.
        /// </summary>
        private void OnOperationComplete(AsyncOperation operation)
        {
            curOperation = null;
            SetFinished();
        }

        /// <summary>
        /// Polls the current operation's progress.
        /// </summary>
        private IEnumerator PollProgress()
        {
            if(curOperation == null)
                yield break;

            while (!IsFinished && curOperation != null)
            {
                SetProgress(curOperation.progress);
                yield return null;
            }
        }
    }
}