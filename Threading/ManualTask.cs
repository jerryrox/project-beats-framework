using System;
using PBFramework.Data.Bindables;

namespace PBFramework.Threading
{
    /// <summary>
    /// An ITask implementation to wrap a lambda action into a task.
    /// Designed for single-time use.
    /// </summary>
    public class ManualTask : ITask
    {
        /// <summary>
        /// Event called on task finish.
        /// </summary>
        public event Action OnFinished;

        protected BindableBool isRevoked = new BindableBool(false);

        protected Action<ManualTask> action;


        /// <summary>
        /// Current listener instance registered when the task was started.
        /// </summary>
        public TaskListener Listener { get; private set; }

        public bool IsFinished { get; protected set; }

        public bool DidRun { get; private set; }

        /// <summary>
        /// Returns whether the task has been revoked.
        /// </summary>
        public IReadOnlyBindable<bool> IsRevoked => isRevoked;


        public ManualTask() : this(null)
        { }

        /// <summary>
        /// Initializes a new ManualTask with the specified action.
        /// The action takes a task listener 
        /// </summary>
        public ManualTask(Action<ManualTask> action)
        {
            this.action = action;
        }

        public void StartTask(TaskListener listener = null)
        {
            if(DidRun)
                return;
            DidRun = true;

            this.Listener = listener;
            if(action == null)
                SetFinished();
            else
                action.Invoke(this);
        }

        public void RevokeTask(bool dispose)
        {
            if(isRevoked.Value)
                return;
            isRevoked.Value = true;
        }

        /// <summary>
        /// Signals that the task is finished.
        /// </summary>
        public void SetFinished()
        {
            if (isRevoked.Value || IsFinished)
                return;
            IsFinished = true;

            Listener?.SetFinished();
            InvokeFinished();
        }

        /// <summary>
        /// Reports the progress of the task.
        /// </summary>
        public void SetProgress(float progress)
        {
            if (isRevoked.Value || IsFinished)
                return;

            Listener?.SetProgress(progress);
        }

        /// <summary>
        /// Invokes OnFinished event.
        /// </summary>
        protected void InvokeFinished()
        {
            OnFinished?.Invoke();
        }
    }

    /// <summary>
    /// Generic version of ManualTask for a return value.
    /// </summary>
    public class ManualTask<T> : ManualTask, ITask<T>
    {
        /// <summary>
        /// Event called when the task has finished with a value.
        /// </summary>
        public new event Action<T> OnFinished;


        public ManualTask(Action<ManualTask<T>> action) :
            base(action == null ? null : new Action<ManualTask>((task) =>
            {
                action.Invoke(task as ManualTask<T>);
            }))
        { }

        public void StartTask(TaskListener<T> listener = null)
        {
            base.StartTask(listener);
        }

        /// <summary>
        /// Signals that the task is finished with a return value.
        /// </summary>
        public void SetFinished(T value)
        {
            if(isRevoked.Value || IsFinished)
                return;
            IsFinished = true;
            
            (base.Listener as TaskListener<T>)?.SetFinished(value);
            InvokeFinished(value);
        }

        /// <summary>
        /// Invokes OnFinished event with a return value.
        /// </summary>
        protected void InvokeFinished(T value)
        {
            base.InvokeFinished();
            OnFinished?.Invoke(value);
        }
    }
}