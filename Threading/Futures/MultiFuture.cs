using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Threading.Futures
{
    public class MultiFuture : Future, IFuture {

        private List<IFuture> futures = new List<IFuture>();


        /// <summary>
        /// Returns the list of futures handled by this future.
        /// </summary>
        public IReadOnlyList<IFuture> Futures => futures;


        /// <summary>
        /// Creates a MultiFuture that waits for a single future to finish.
        /// </summary>
        public MultiFuture(IFuture future = null)
        {
            AddFuture(future);
            StartWait();
        }

        /// <summary>
        /// Creates a MultiFuture that waits for the specified range of futures to finish.
        /// </summary>
        public MultiFuture(IEnumerable<IFuture> futures)
        {
            foreach(var future in futures)
                AddFuture(future);
            StartWait();
        }

        /// <summary>
        /// Disposes all children futures along with this future.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            futures.ForEach(f => f.Dispose());
        }

        /// <summary>
        /// Adds the specified future to waiting list if not null.
        /// </summary>
        private void AddFuture(IFuture future)
        {
            if (future != null)
            {
                futures.Add(future);
            }
        }

        /// <summary>
        /// Starts waiting for the current list of futures to finish.
        /// </summary>
        private void StartWait()
        {
            StartRunning(futures.Count == 0 ? null : new Action(() =>
            {
                foreach (var future in futures)
                {
                    future.Progress.OnNewValue += OnProgressUpdate;
                    future.IsCompleted.OnNewValue += OnCompleted;
                }
                // Trigger initial check.
                OnProgressUpdate(0f);
                OnCompleted(false);
            }));
        }

        /// <summary>
        /// Event called on new progress value update from a child future.
        /// </summary>
        private void OnProgressUpdate(float _)
        {
            float progress = futures.Select(f => f.Progress.Value).Sum() / futures.Count;
            ReportProgress(progress);
        }

        /// <summary>
        /// Event called when a child future's inner task has finished its job.
        /// </summary>
        private void OnCompleted(bool _)
        {
            bool completed = futures.All(f => f.IsCompleted.Value);
            if(completed)
                OnComplete(null);
        }
    }
}