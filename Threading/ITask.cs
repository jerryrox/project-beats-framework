namespace PBFramework.Threading
{
    /// <summary>
    /// A general interface indicating that the object's task can be controlled.
    /// </summary>
    public interface ITask {

        /// <summary>
        /// Returns whether the task has been run.
        /// </summary>
        bool DidRun { get; }

        /// <summary>
        /// Returns whether the task has already been finished.
        /// </summary>
        bool IsFinished { get; }


        /// <summary>
        /// Starts the task.
        /// </summary>
        void StartTask(TaskListener listener = null);

        /// <summary>
        /// Stops any on-going task.
        /// Specify dispose = true to tell the task whether the object should be disposed.
        /// However, there is no guarantee the implementation will be reusable with dispose = false.
        /// </summary>
        void RevokeTask(bool dispose);
    }

    /// <summary>
    /// Generic version of ITask.
    /// </summary>
    public interface ITask<T> : ITask {

        /// <summary>
        /// Starts the task.
        /// </summary>
        void StartTask(TaskListener<T> listener = null);
    }
}