using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Services;

namespace PBFramework.Threading
{
    /// <summary>
    /// An ITimer implementation which runs on the Unity thread.
    /// </summary>
    public class SynchronizedTimer : ITimer
    {
        public event Action<ITimer> OnFinished;
        event Action<ITimer> IPromise<ITimer>.OnFinishedResult
        {
            add => OnFinished += value;
            remove => OnFinished -= value;
        }
        event Action IPromise.OnFinished
        {
            add => OnFinished += delegate { value(); };
            remove => OnFinished -= delegate { value(); };
        }

        public event Action<float> OnProgress;

        private Coroutine timerCoroutine;


        /// <summary>
        /// Whether the timer should wait a frame before starting to wait.
        /// </summary>
        public bool WaitFrameOnStart { get; set; } = false;

        public float Limit { get; set; } = float.MaxValue;

        public float Current { get; set; }

        public bool IsRunning => timerCoroutine != null;

        public float Progress { get; set; }

        public ITimer Result => this;
        object IPromise.RawResult => this;

        public bool IsFinished => Current >= Limit;


        public void Start()
        {
            if(IsRunning) return;

            // Start coroutine.
            timerCoroutine = UnityThread.StartCoroutine(TimerRoutine());
        }

        public void Revoke() => Stop();

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
                    // Finished
                    OnFinished?.Invoke(this);
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
            if(Limit <= 0)
                Progress = 0f;
            else
            {
                if(Current >= Limit)
                    Progress = 1f;
                else
                    Progress = Current / Limit;
            }
            OnProgress?.Invoke(Progress);
        }
    }
}