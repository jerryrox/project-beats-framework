using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PBFramework.Threading
{
    /// <summary>
    /// An ITimer implementation which runs on a separate thread.
    /// </summary>
    public class AsynchronizedTimer : ITimer {
    
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

        private int delta;
        private object locker = new object();
        private float limit = float.MaxValue;
        private float current;

        private bool shouldRun;


        public float Limit
        {
            get { lock (locker) { return limit; } }
            set { lock (locker) { limit = value; } }
        }

        public float Current
        {
            get { lock (locker) { return current; } }
            set { lock (locker) { current = value; } }
        }

        public bool IsRunning
        {
            get { lock (locker) { return shouldRun; } }
        }

        public float Progress { get; set; }

        public ITimer Result => this;
        object IPromise.Result => this;

        public bool IsFinished
        {
            get { lock (locker) { return current >= limit; } }
        }


        /// <summary>
        /// Initializes the timer with specified delta in milliseconds.
        /// The timer will check for its progress every this interval.
        /// Lower value indicates higher precision but may result in higher performance cost.
        /// </summary>
        public AsynchronizedTimer(int delta = 20)
        {
            this.delta = delta;
        }

        public void Start()
        {
            lock (locker)
            {
                if(IsRunning) return;

                shouldRun = true;
                StartInternal();
            }
        }

        public void Revoke() => Stop();

        public void Pause()
        {
            lock (locker)
            {
                if(!IsRunning) return;

                shouldRun = false;
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                Pause();

                // Reset time.
                current = 0f;
                ReportCurrent();
            }
        }

        private void StartInternal()
        {
            Task.Run(() =>
            {
                DateTime startDT = new DateTime(1970, 1, 1);
                double lastTime = (DateTime.Now - startDT).TotalMilliseconds;
                while (true)
                {
                    Thread.Sleep(delta);
                    lock (locker)
                    {
                        // Calculate actual delta time.
                        double newTime = (DateTime.Now - startDT).TotalMilliseconds;
                        
                        // Add current time.
                        current += (float)(newTime - lastTime) * 0.001f;
                        lastTime = newTime;

                        // Check if reached the limit.
                        if (current >= limit)
                        {
                            // Clamp
                            current = limit;
                            // Notify asynchronously.
                            ReportCurrent();
                            // Finished
                            OnFinished?.Invoke(this);
                            // Stop the routine.
                            Pause();
                            break;
                        }
                        else
                        {
                            // Notify asynchronously.
                            ReportCurrent();
                        }

                        // Break out of loop if cancelled.
                        if(!shouldRun)
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// Reports the current progress by dividing Current time over Limit.
        /// </summary>
        private void ReportCurrent()
        {
            lock (locker)
            {
                if(limit <= 0)
                    Progress = 0f;
                else
                {
                    if(current >= limit)
                        Progress = 1f;
                    else
                        Progress = current / limit;
                }
                OnProgress?.Invoke(Progress);
            }
        }
    }
}