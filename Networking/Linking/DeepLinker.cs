using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Networking.Linking
{
    public class DeepLinker {

        /// <summary>
        /// Delegate for handling link events when a specific link has been activated.
        /// </summary>
        public delegate void HookHandler(WebLink link);

        private Dictionary<string, HookHandler> hooks = new Dictionary<string, HookHandler>();


        public DeepLinker()
        {
            // Hook deep link event
            Application.deepLinkActivated += Emit;
        }

        /// <summary>
        /// Starts listening to links of specified path.
        /// </summary>
        public void LinkPath(string path, HookHandler handler)
        {
            if(handler == null || path == null)
                return;

            path = WebLink.ExtractPath(path);
            hooks[path] = handler;
        }

        /// <summary>
        /// Emits a deeplink event using the specified path.
        /// </summary>
        public void Emit(string path)
        {
            WebLink webPath = new WebLink(path);
            if (hooks.TryGetValue(webPath.Path, out HookHandler handler))
            {
                handler?.Invoke(webPath);
            }
        }

        /// <summary>
        /// Returns whether there is a hook registered for the specified path.
        /// </summary>
        public bool IsLinked(string path)
        {
            path = WebLink.ExtractPath(path);
            return hooks.ContainsKey(path);
        }
    }
}