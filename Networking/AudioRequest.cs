using System;
using UnityEngine;
using UnityEngine.Networking;
using PBFramework.Data.Bindables;
using PBFramework.Threading.Futures;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Networking
{
    public class AudioRequest : AssetRequest, IControlledFuture<AudioClip> {

        private Bindable<AudioClip> output = new Bindable<AudioClip>(null)
        {
            TriggerWhenDifferent = true
        };

        private DownloadHandlerAudioClip downloadHandler;

        private bool isStream;


        public IReadOnlyBindable<AudioClip> Output => output;

        public override object RawResult => response?.AudioData;


        public AudioRequest(string url, bool isStream = false) : base(url)
        {
            this.isStream = isStream;
        }

        protected override void DisposeSoft()
        {
            base.DisposeSoft();
            if (!IsDisposed.Value)
            {
                output.Value = null;
            }
            if (downloadHandler != null)
                downloadHandler = null;
        }

        protected override void EvaluateResponse()
        {
            output.Value = response.AudioData;
        }

        protected override UnityWebRequest CreateRequest(string url)
        {
            var request = UnityWebRequestMultimedia.GetAudioClip(url, GetAudioType(url));
            downloadHandler = ((DownloadHandlerAudioClip)request.downloadHandler);
            downloadHandler.streamAudio = isStream;
            downloadHandler.compressed = isStream;
            return request;
        }

        /// <summary>
        /// Determines the desired audio type from specified url.
        /// </summary>
        private AudioType GetAudioType(string url)
        {
            if (url.EndsWith("mp3", StringComparison.OrdinalIgnoreCase))
            {
                return AudioType.MPEG;
            }
            else if (url.EndsWith("ogg", StringComparison.OrdinalIgnoreCase))
            {
                return AudioType.OGGVORBIS;
            }
            else if (url.EndsWith("wav", StringComparison.OrdinalIgnoreCase))
            {
                return AudioType.WAV;
            }
            Logger.Log($"AudioRequest.GetAudioType - Unknown audio type for url: {url}");
            return AudioType.UNKNOWN;
        }
    }
}