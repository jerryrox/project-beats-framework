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

        private Coroutine timerCoroutine;


        public float Limit { get; set; } = float.MaxValue;

        public float Current { get; set; }

        public bool IsRunning => timerCoroutine != null;

        public IProgress<float> Progress { get; set; }

        public ITimer Result => this;
        object IPromise.Result => this;

        public bool IsFinished => Current >= Limit;


        public void Start()
        {
            if(IsRunning) return;

            // Start coroutine.
            timerCoroutine = UnityThreadService.StartCoroutine(TimerRoutine());
        }

        public void Revoke() => Stop();

        public void Pause()
        {
            if(!IsRunning) return;

            // Stop coroutine.
            UnityThreadService.StopCoroutine(timerCoroutine);
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
                    Progress?.Report(1f);
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
                Progress?.Report(0f);
            else
                Progress?.Report(Current / Limit);
        }
    }
}