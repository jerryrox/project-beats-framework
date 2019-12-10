using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;
using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Assets.Atlasing
{
    /// <summary>
    /// Implementation of IAtlas for assets stored in resources directory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceAtlas<T> : IAtlas<T>
        where T : Object
    {
        protected readonly Dictionary<string, T> atlas = new Dictionary<string, T>();

        
        /// <summary>
        /// Returns whether the atlas has been disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }


        public virtual void Set(string name, T obj)
        {
            if(IsDisposed) throw new ObjectDisposedException(nameof(ResourceAtlas<T>));

            atlas[name] = obj;
        }

        public virtual T Get(string name)
        {
            if(IsDisposed) throw new ObjectDisposedException(nameof(ResourceAtlas<T>));

            // If exists in atlas, return it.
            if(atlas.TryGetValue(name, out T result))
                return result;
            
            // If doesn't exist, load from resources.
            var loaded = LoadFromResources(name);
            if (loaded == null)
            {
                Logger.LogWarning($"ResourceAtlas.Get - Could not load resource ({name}) of type ({nameof(T)})");
            }
            else
            {
                Set(name, loaded);
            }
            return loaded;
        }

        public virtual bool Contains(string name)
        {
            if(IsDisposed) throw new ObjectDisposedException(nameof(ResourceAtlas<T>));

            return atlas.ContainsKey(name);
        }

        public void Dispose()
        {
            if(IsDisposed) return;

            IsDisposed = true;
            foreach(var obj in atlas.Values)
                Object.Destroy(obj);
            atlas.Clear();
        }

        /// <summary>
        /// Attempts to load missing resource of specified name.
        /// </summary>
        protected virtual T LoadFromResources(string name)
        {
            return Resources.Load<T>(name) as T;
        }
    }
}