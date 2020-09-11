namespace PBFramework.Threading
{
    /// <summary>
    /// A general interface indicating that the object's task can be controlled.
    /// </summary>
    public interface ITask {

        /// <summary>
        /// Starts the task.
        /// </summary>
        void StartTask(TaskListener listener = null);

        /// <summary>
        /// Stops any on-going task.
        /// Specify "dispose" to tell the task whether the object should be disposed.
        /// </summary>
        void RevokeTask(bool dispose = false);
    }

    /// <summary>
    /// Generic version of ITask.
    /// </summary>
    public interface ITask<T> : ITask {

        void StartTask(TaskListener<T> listener = null);
    }
}