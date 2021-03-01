namespace PBFramework.Debugging
{
    /// <summary>
    /// Interface of a servicer object which handles logging of debug messages.
    /// </summary>
    public interface ILogService {

        /// <summary>
        /// Logs the specified verbose message.
        /// </summary>
        void LogVerbose(object message);

        /// <summary>
        /// Logs the specified info message.
        /// </summary>
        void LogInfo(object message);
        
        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        void LogWarning(object message);
        
        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        void LogError(object message);
    }
}