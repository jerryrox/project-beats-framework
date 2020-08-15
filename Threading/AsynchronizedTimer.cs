using System;
using System.Threading;
using System.Threading.Tasks;
using PBFramework.Data.Bindables;

namespace PBFramework.Threading
{
    /// <summary>
    /// An ITimer implementation which runs on a separate thread.
    /// </summary>
    public class AsynchronizedTimer : ITimer {
    
        private BindableFloat progress = new BindableFloat(0f);
        private BindableBool isCompleted = new BindableBool(false)
        {
            TriggerWhenDifferent = true
        };

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
            get { lock (locker) { return shouldRun; }; }
        }

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

        public void Dispose() => Stop();

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
                if (limit <= 0)
                {
                    progress.Value = 0f;
                    isCompleted.Value = false;
                }
                else
                {
                    if (current >= limit)
                    {
                        progress.Value = 1f;
                        isCompleted.Value = true;
                    }
                    else
                    {
                        progress.Value = current / limit;
                        isCompleted.Value = false;
                    }
                }
            }
        }
    }
}