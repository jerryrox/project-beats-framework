using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Platforms.OsxEditor;

namespace PBFramework.Platforms
{
    public static class PlatformHost {

        /// <summary>
        /// Creates a new platform host instance for current platform.
        /// </summary>
        public static IPlatformHost CreateHost()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    return new OsxEditorHost();

                default:
                    throw new Exception($"Unable to create platform host for ({Application.platform.ToString()})");
            }
        }
    }
}