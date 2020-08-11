namespace PBFramework.Threading.Futures
{
    /// <summary>
    /// A variant of IFuture which allows the consumer to control when the inner task should be started.
    /// Methods should return IControlledFuture or its typed variant if the inner task will be run by the consumer manually.
    /// </summary>
    public interface IControlledFuture : IFuture {

        /// <summary>
        /// Starts running the associated inner task.
        /// </summary>
        void Start();
    }

    /// <summary>
    /// A generic version of IControlledFuture for an additional property for the output value.
    /// </summary>
    public interface IControllerFuture<T> : IControlledFuture, IFuture<T>
    {
    }
}