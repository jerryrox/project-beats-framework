using System;
using System.Runtime.CompilerServices;

namespace PBFramework.Threading.Futures
{
    public class FutureAwaiter : INotifyCompletion
    {
        private IFuture future;
        private Action<Action> receiveContinuation;


        public bool IsCompleted
        {
            get => future.IsCompleted.Value;
        }


        public FutureAwaiter(IFuture future, Action<Action> receiveContinuation)
        {
            if(future == null)
                throw new ArgumentNullException(nameof(future));
            if(receiveContinuation == null)
                throw new ArgumentNullException(nameof(receiveContinuation));

            this.future = future;
            this.receiveContinuation = receiveContinuation;
        }

        public void GetResult() {}

        public void OnCompleted(Action continuation)
        {
            receiveContinuation(continuation);
        }
    }
}