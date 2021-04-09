using System;

namespace PBFramework.Utils
{
    public static class DateUtils {

        /// <summary>
        /// Returns the timestamp of the specified date time in milliseconds since epoch.
        /// </summary>
        public static long GetTimestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }
    }
}