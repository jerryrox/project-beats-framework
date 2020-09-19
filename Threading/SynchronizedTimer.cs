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
        public event Action OnFinished;

        public event Action<float> OnProgress;

        private Coroutine timerCoroutine;


        /// <summary>
        /// Whether the timer should wait a frame before starting to wait.
        /// </summary>
        public bool WaitFrameOnStart { get; set; } = false;

        public float Limit { get; set; } = float.MaxValue;

        public float Current { get; set; }

        public float Progress => Limit == 0 ? 1 : Current / Limit;

        public bool IsRunning => timerCoroutine != null;

        public bool IsFinished => Current >= Limit;


        public void Start()
        {
            if(IsRunning) return;

            // Start coroutine.
            timerCoroutine = UnityThread.StartCoroutine(TimerRoutine());
        }

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
                InvokeProgress();
            }
            else
            {
                if (Current >= Limit)
                {
                    InvokeProgress();
                    InvokeFinished();
                }
                else
                {
                    InvokeProgress();
                }
            }
        }

        /// <summary>
        /// Invokes OnProgress event.
        /// </summary>
        private void InvokeProgress()
        {
            OnProgress?.Invoke(Progress);
        }

        /// <summary>
        /// Invokes OnFinished event.
        /// </summary>
        private void InvokeFinished()
        {
            OnFinished?.Invoke();
        }
    }
}