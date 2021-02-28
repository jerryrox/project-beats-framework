using UnityEngine;

namespace PBFramework.Debugging
{
    /// <summary>
    /// Log servicer using Unity's Debugger.
    /// </summary>
    public class DebugLogService : ILogService
    {
        public void LogVerbose(object message)
        {
            Debug.Log(message);
        }

        public void LogInfo(object message)
        {
            Debug.Log(message);
        }

        public void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}