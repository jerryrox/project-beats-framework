﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Debugging
{
    /// <summary>
    /// Static instance which handles logging of messages in application scope.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Event called when a warning message has been dispatched to the logger.
        /// </summary>
        public static event Action<Object> OnWarning;

        /// <summary>
        /// Event called when an error message has been dispatched to the logger.
        /// </summary>
        public static event Action<Object> OnError;

        /// <summary>
        /// List of all log servicer registered to the logger.
        /// </summary>
        private static List<ILogService> LogServices;

        /// <summary>
        /// Concurrency lock.
        /// </summary>
        private static Object Lock = new Object();


        static Logger()
        {
            LogServices = new List<ILogService>();

            // By default, add the DebugLogServices only during editor runtime.
#if UNITY_EDITOR
            LogServices.Add(new DebugLogService());
#endif
        }

        /// <summary>
        /// Logs a normal message.
        /// </summary>
        public static void Log(Object message)
        {
            lock (Lock)
            {
                LogServices.ForEach(s => s.Log(message));
            }
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        public static void LogWarning(Object message)
        {
            lock (Lock)
            {
                LogServices.ForEach(s => s.LogWarning(message));
                OnWarning?.Invoke(message);
            }
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        public static void LogError(Object message)
        {
            lock (Lock)
            {
                LogServices.ForEach(s => s.LogError(message));
                OnError?.Invoke(message);
            }
        }

        /// <summary>
        /// Registers the specified log servicer.
        /// </summary>
        public static void Register(ILogService service)
        {
            lock (Lock)
            {
                LogServices.Add(service);
            }
        }
    }
}