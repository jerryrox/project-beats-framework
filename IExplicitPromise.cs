namespace PBFramework
{
    /// <summary>
    /// IPromise extension which assumes the requirement for explicit Start()/Revoke() calls.
    /// </summary>
    public interface IExplicitPromise : IPromise
    {
        /// <summary>
        /// Starts the process of the underlying implementation.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the process of the underlying implementation.
        /// </summary>
        void Revoke();
    }

    public interface IExplicitPromise<T> : IExplicitPromise, IPromise<T> {
    
        
    }
}