using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.Allocation.Recyclers
{
    public static class RecyclableExtensions
    {
        /// <summary>
        /// Makes the specified recyclable object return to its recycler.
        /// </summary>
        public static void ReturnToRecycler<T>(this T context) where T : class, IRecyclable<T>
        {
            if (context.Recycler == null)
            {
                Logger.LogWarning("RecyclableExtensions.ReturnToRecycler - The context's recycler is null!");
                return;
            }
            context.Recycler.Return(context);
        }
}
}