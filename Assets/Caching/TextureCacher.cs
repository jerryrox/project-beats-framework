using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Networking;

namespace PBFramework.Assets.Caching
{
    /// <summary>
    /// AssetCacher variant for loading a Texture2D object from local or remote source.
    /// </summary>
    public class TextureCacher : AssetCacher<Texture2D> {

        private bool nonReadable;


        public TextureCacher(bool nonReadable = true)
        {
            this.nonReadable = nonReadable;
        }

        protected override IExplicitPromise<Texture2D> CreateRequest(string key)
        {
            return new TextureRequest(key, nonReadable);
        }
    }
}