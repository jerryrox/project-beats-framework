using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Allocation.Caching;

using Object = UnityEngine.Object;

namespace PBFramework.Assets.Caching
{
    /// <summary>
    /// Cacher extension which assumes TValue to be of a Unity object which can be destroyed via Object.Destroy call.
    /// </summary>
    public abstract class AssetCacher<TKey, TValue> : Cacher<TKey, TValue>
        where TKey : class
        where TValue : Object {

        protected override void DestroyData(TValue data) => Object.Destroy(data);
    }

    /// <summary>
    /// Extension of AssetCacher which assumes TKey as a string value.
    /// </summary>
    public abstract class AssetCacher<T> : AssetCacher<string, T>
        where T : Object
    {
    }
}