using System.Linq;

namespace PBFramework
{
    /// <summary>
    /// Promise which yields for multiple promises at once.
    /// Note that this promise itself does not provide a non-null result.
    /// </summary>
    public class MultiPromise : ProxyPromise {

        private int finishedCount = 0;


        /// <summary>
        /// Returns the array of all promises currently being processed.
        /// </summary>
        public IExplicitPromise[] Promises { get; private set; }


        public MultiPromise(params IExplicitPromise[] promises)
        {
            // Set default value
            if(promises == null) promises = new IExplicitPromise[0];

            Promises = promises;

            // Bind proxied actions
            startAction = () =>
            {
                if (promises.Length == 0)
                {
                    SetProgress(1f);
                    Resolve(null);
                }
                else
                    promises.ForEach(p => p.Start());
            };
            revokeAction = () => promises.ForEach(p => p.Revoke());

            // Listen to events
            promises.ForEach(p =>
            {
                p.OnFinished += () =>
                {
                    finishedCount++;
                    if(finishedCount == promises.Length)
                        Resolve(null);
                };
                p.OnProgress += (progress) =>
                {
                    SetProgress(promises.Sum(p2 => p2.Progress) / promises.Length);
                };
            });
        }
    }
}