using System.Linq;
using System.Collections.Generic;

namespace PBFramework
{
    /// <summary>
    /// Promise which yields for multiple promises at once.
    /// Note that this promise itself does not provide a non-null result.
    /// </summary>
    public class MultiPromise : ProxyPromise {

        private int finishedCount = 0;

        private List<IExplicitPromise> promises = new List<IExplicitPromise>();


        /// <summary>
        /// Returns the array of all promises currently being processed.
        /// </summary>
        public IReadOnlyList<IExplicitPromise> Promises => promises;


        public MultiPromise(IEnumerable<IExplicitPromise> promises)
        {
            foreach (var promise in promises)
            {
                if(promise != null)
                    this.promises.Add(promise);
            }

            // Bind proxied actions
            StartAction = (promise) =>
            {
                if (this.promises.Count == 0)
                {
                    SetProgress(1f);
                    Resolve(null);
                }
                else
                    this.promises.ForEach(p => p.Start());
            };
            RevokeAction = () => this.promises.ForEach(p => p.Revoke());

            // Listen to events
            this.promises.ForEach(p =>
            {
                p.OnFinished += () =>
                {
                    finishedCount++;
                    if (finishedCount == this.promises.Count)
                        Resolve(null);
                };
                p.OnProgress += (progress) =>
                {
                    SetProgress(this.promises.Sum(p2 => p2.Progress) / this.promises.Count);
                };
            });
        }
    }
}