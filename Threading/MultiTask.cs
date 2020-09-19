using System;
using System.Linq;
using System.Collections.Generic;
using PBFramework.Data.Bindables;

namespace PBFramework.Threading
{
    /// <summary>
    /// ITask implementation to represent a single task that awaits for its list of tasks.
    /// Designed for single-time use.
    /// </summary>
    public class MultiTask : ITask
    {
        /// <summary>
        /// Event called on task finish.
        /// </summary>
        public event Action OnFinished;

        private BindableBool isRevoked = new BindableBool(false);

        private List<ITask> tasks = new List<ITask>();


        /// <summary>
        /// Current listener instance registered when the task was started.
        /// </summary>
        public TaskListener Listener { get; private set; }

        /// <summary>
        /// Returns the list of tasks this object is waiting for.
        /// </summary>
        public IReadOnlyList<ITask> Tasks => tasks.AsReadOnly();

        public bool IsFinished { get; private set; }

        public bool DidRun { get; private set; }

        /// <summary>
        /// Returns whether the task has been revoked.
        /// </summary>
        public IReadOnlyBindable<bool> IsRevoked => isRevoked;


        public MultiTask() : this(null) { }

        public MultiTask(IEnumerable<ITask> tasks)
        {
            if(tasks != null)
                this.tasks.AddRange(tasks);
        }

        public void StartTask(TaskListener listener = null)
        {
            if(DidRun)
                return;
            DidRun = true;

            this.Listener = listener;
            if (listener != null)
                listener.HasOwnProgress = false;

            EvaluateFinished();
            if(IsFinished)
                return;

            tasks.ForEach(t => 
            {
                if (!t.IsFinished)
                {
                    var subListener = listener?.CreateSubListener() ?? new TaskListener();
                    subListener.OnFinished += EvaluateFinished;
                    t.StartTask(subListener);
                }
            });
        }

        public void RevokeTask(bool dispose)
        {
            if(isRevoked.Value)
                return;
            isRevoked.Value = true;
            
            tasks.ForEach(t => t.RevokeTask(dispose));
        }

        private void EvaluateFinished()
        {
            if (!IsFinished && (tasks.Count == 0 || tasks.All(t => t.IsFinished)))
                SetFinished();
        }

        /// <summary>
        /// Signals that the task is finished.
        /// </summary>
        private void SetFinished()
        {
            if (isRevoked.Value || IsFinished)
                return;
            IsFinished = true;

            Listener?.SetFinished();
            OnFinished?.Invoke();
        }
    }
}