using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Networking
{
    /// <summary>
    /// Extension of WebRequest for use with Unity asset loading.
    /// </summary>
    public abstract class AssetRequest : WebRequest {
    
        protected AssetRequest(string url, int timeout = 60, int retryCount = 1) :
            base(
                $"{(url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? "" : (url.StartsWith("file:", StringComparison.OrdinalIgnoreCase) ? "" : "file://"))}{url}",
                timeout: timeout,
                retryCount: retryCount
            )
        { }
    }
}