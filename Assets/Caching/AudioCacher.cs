using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBFramework.Networking;

namespace PBFramework.Assets.Caching
{
    /// <summary>
    /// AssetCacher variant for loading AudioClips from local or remote source.
    /// </summary>
    public class AudioCacher : AssetCacher<AudioClip> {

        private bool stream;


        public AudioCacher(bool stream = true)
        {
            this.stream = stream;
        }

        protected override IExplicitPromise<AudioClip> CreateRequest(string key)
        {
            return new AudioRequest(key, stream);
        }

    }
}