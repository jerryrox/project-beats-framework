using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Threading
{
    /// <summary>
    /// A container object which holds data about a task.
    /// </summary>
    public class TaskListener
    {
        /// <summary>
        /// Event called when a new progress value is reported from the task.
        /// Always called on the main thread.
        /// </summary>
        public event Action<float> OnProgress;

        /// <summary>
        /// Event called when this task has been finished.
        /// There is no guarantee the sublisteners' tasks have been finished too.
        /// Always called on the main thread.
        /// </summary>
        public event Action OnFinished;

        protected List<TaskListener> subListeners = new List<TaskListener>();


        /// <summary>
        /// Returns the total progress of this and the sub listeners' tasks.
        /// </summary>
        public float TotalProgress { get; private set; }

        /// <summary>
        /// Returns the progress of this specific task.
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// Whether the task of this listener has been finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Whether events should always be called on the main thread.
        /// </summary>
        public SynchronizedBool CallEventOnMainThread { get; } = new SynchronizedBool(true);

        /// <summary>
        /// Returns the total number of sub tasks branched out under this task.
        /// </summary>
        public int SubListenerCount
        {
            get
            {
                lock (subListeners)
                {
                    return subListeners.Sum(s => s.SubListenerCount + 1);
                }
            }
        }

        /// <summary>
        /// Creates a new listener branched out from this listener to represent a child task.
        /// Returns null if listener has already finished its task.
        /// </summary>
        public TaskListener CreateSubListener()
        {
            if (IsFinished)
                return null;

            var listener = new TaskListener();
            AddSubListener(listener);
            return listener;
        }

        /// <summary>
        /// Creates a new generic listener branched out from this listener to represent a child task.
        /// Returns null if listener has already finished its task.
        /// </summary>
        public TaskListener<T> CreateSubListener<T>()
        {
            if (IsFinished)
                return null;

            var listener = new TaskListener<T>();
            AddSubListener(listener);
            return listener;
        }

        /// <summary>
        /// Sets the progress of this listener.
        /// </summary>
        public void SetProgress(float progress)
        {
            if (IsFinished)
                return;

            this.Progress = Mathf.Clamp01(progress);
            EvaluateTotalProgress();
        }

        /// <summary>
        /// Marks finished state on this listener.
        /// </summary>
        public void SetFinished()
        {
            if (IsFinished)
                return;

            this.IsFinished = true;
            this.Progress = 1f;
            EvaluateTotalProgress();
            InvokeFinished();
        }

        /// <summary>
        /// Resets the listener's state for reuse.
        /// </summary>
        public virtual void ResetState()
        {
            TotalProgress = 0;
            Progress = 0;
            IsFinished = false;
            subListeners.Clear();
        }

        /// <summary>
        /// Adds the specified listener while setting up as a sub listener under this instance.
        /// </summary>
        private void AddSubListener(TaskListener listener)
        {
            listener.OnProgress += (p) => EvaluateTotalProgress();
            lock (subListeners)
            {
                subListeners.Add(listener);
                EvaluateTotalProgress();
            }
        }

        /// <summary>
        /// Invokes progress change event.
        /// </summary>
        private void InvokeProgress(float progress)
        {
            if (OnProgress != null)
            {
                if (CallEventOnMainThread.Value)
                {
                    UnityThread.DispatchUnattended(() =>
                    {
                        OnProgress?.Invoke(progress);
                        return null;
                    });
                }
                else
                    OnProgress?.Invoke(progress);
            }
        }

        /// <summary>
        /// Invokes finished event.
        /// </summary>
        private void InvokeFinished()
        {
            if (OnFinished != null)
            {
                if (CallEventOnMainThread.Value)
                {
                    UnityThread.DispatchUnattended(() =>
                    {
                        OnFinished?.Invoke();
                        return null;
                    });
                }
                else
                    OnFinished?.Invoke();
            }
        }

        /// <summary>
        /// Calculates the total progress.
        /// </summary>
        private void EvaluateTotalProgress()
        {
            float totalProgress = this.Progress;
            lock (subListeners)
            {
                totalProgress += subListeners.Sum(l => l.TotalProgress);
                totalProgress /= (subListeners.Count + 1);

                this.TotalProgress = totalProgress;
            }
            InvokeProgress(totalProgress);
        }
    }

    /// <summary>
    /// Generic version of TaskListener for a return value.
    /// </summary>
    public class TaskListener<T> : TaskListener
    {
        /// <summary>
        /// Generic version of OnFinished for output data.
        /// </summary>
        public new event Action<T> OnFinished;


        /// <summary>
        /// Output data from the task listening to.
        /// </summary>
        public T Value { get; private set; }


        public TaskListener()
        {
            base.OnFinished += () => this.OnFinished?.Invoke(Value);
        }

        public override void ResetState()
        {
            base.ResetState();
            Value = default;
        }

        /// <summary>
        /// Sets the output data from the task.
        /// </summary>
        public void SetValue(T value)
        {
            if(IsFinished)
                return;

            this.Value = value;
        }

        /// <summary>
        /// Flags finished state with a return value T.
        /// </summary>
        public void SetFinished(T value)
        {
            SetValue(value);
            base.SetFinished();
        }
    }
}