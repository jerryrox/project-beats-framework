using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Threading
{
    public class FramedTimer : ITimer {
    
        public event Action OnFinished;

        public event Action<float> OnProgress;

        private Coroutine timerCoroutine;
        private int limitFrames = int.MaxValue;
        private int curFrames = 0;


        public float Limit
        {
            get => limitFrames;
            set
            {
                int intValue = (int)value;
                if ((value - intValue) > 0.5)
                    limitFrames = intValue + 1;
                else
                    limitFrames = intValue;
            }
        }

        public float Current
        {
            get => curFrames;
            set
            {
                int intValue = (int)value;
                if ((value - intValue) > 0.5)
                    curFrames = intValue + 1;
                else
                    curFrames = intValue;
            }
        }

        public float Progress => Limit == 0 ? 1 : Current / Limit;

        public bool IsRunning => timerCoroutine != null;

        public bool IsFinished => Current >= Limit;


        public void Start()
        {
            if (IsRunning)
                return;

            timerCoroutine = UnityThread.StartCoroutine(TimerRoutine());
        }

        public void Pause()
        {
            if (!IsRunning)
                return;

            UnityThread.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        public void Stop()
        {
            Pause();

            curFrames = 0;
            ReportCurrent();
        }

        /// <summary>
        /// Coroutine which counts the elapsed frames.
        /// </summary>
        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                curFrames++;

                // If reached the end
                if (curFrames >= limitFrames)
                {
                    curFrames = limitFrames;
                    ReportCurrent();
                    Pause();
                    yield break;
                }
                else
                {
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