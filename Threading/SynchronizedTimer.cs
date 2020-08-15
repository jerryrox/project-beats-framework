using System;
using System.Collections;
using PBFramework.Data.Bindables;
using UnityEngine;

namespace PBFramework.Threading
{
    /// <summary>
    /// An ITimer implementation which runs on the Unity thread.
    /// </summary>
    public class SynchronizedTimer : ITimer
    {
        private BindableFloat progress = new BindableFloat(0f);
        private BindableBool isCompleted = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };
        private Coroutine timerCoroutine;


        /// <summary>
        /// Whether the timer should wait a frame before starting to wait.
        /// </summary>
        public bool WaitFrameOnStart { get; set; } = false;

        public float Limit { get; set; } = float.MaxValue;

        public float Current { get; set; }

        public bool IsRunning => timerCoroutine != null;

        public IReadOnlyBindable<float> Progress => progress;

        public IReadOnlyBindable<bool> IsCompleted => isCompleted;

        public IReadOnlyBindable<bool> IsDisposed => null;
        
        public IReadOnlyBindable<Exception> Error => null;

        public bool DidRun => IsRunning;

        public bool IsThreadSafe
        {
            get => true;
            set => throw new NotSupportedException();
        }


        public void Start()
        {
            if(IsRunning) return;

            // Start coroutine.
            timerCoroutine = UnityThread.StartCoroutine(TimerRoutine());
        }

        public void Dispose() => Stop();

        public void Pause()
        {
            if(!IsRunning) return;

            // Stop coroutine.
            UnityThread.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        public void Stop()
        {
            Pause();

            // Reset state.
            Current = 0f;
            ReportCurrent();
        }

        /// <summary>
        /// Coroutine which runs on the UnityThreadService.
        /// </summary>
        IEnumerator TimerRoutine()
        {
            if(WaitFrameOnStart)
                yield return null;
                
            while (true)
            {
                // Increase current time.
                Current += Time.deltaTime;

                // Check if reached the limit.
                if (Current >= Limit)
                {
                    // Clamp
                    Current = Limit;
                    // Notify
                    ReportCurrent();
                    // Stop the routine.
                    Pause();
                    yield break;
                }
                else
                {
                    // Notify
                    ReportCurrent();
                }
                yield return null;
            }
        }

        /// <summary>
        /// Reports the current progress by dividing Current time over Limit.
        /// </summary>
        private void ReportCurrent()
        {
            if (Limit <= 0)
            {
                progress.Value = 0f;
                isCompleted.Value = false;
            }
            else
            {
                if (Current >= Limit)
                {
                    progress.Value = 1f;
                    isCompleted.Value = true;
                }
                else
                {
                    progress.Value = Current / Limit;
                    isCompleted.Value = false;
                }
            }
        }
    }
}