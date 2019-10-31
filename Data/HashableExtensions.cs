using System;

namespace PBFramework.Data
{
    public static class HashableExtensions
    {
        /// <summary>
        /// Calculates the hash code of this object.
        /// </summary>
        public static int CalculateHash(this IHashable context)
        {
            unchecked
            {
                int hash = 486187739;
                foreach(var param in context.GetHashParams())
                {
                    if(param != null)
                        hash = (hash * 16777619) ^ param.GetHashCode();
                }
                return context.HashCode = hash;
            }
        }
    }
}