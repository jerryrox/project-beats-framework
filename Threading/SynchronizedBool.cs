using System.Threading;

namespace PBFramework.Threading
{
    /// <summary>
    /// Representation of a thread-safe boolean value.
    /// </summary>
    public class SynchronizedBool {

        private long flag = 0;


        /// <summary>
        /// The actual boolean value.
        /// </summary>
        public bool Value
        {
            get => Interlocked.Read(ref flag) == 1;
            set => Interlocked.Exchange(ref flag, value ? 1 : 0);
        }


        public SynchronizedBool(bool initialValue)
        {
            flag = initialValue ? 1 : 0;
        }
    }
}