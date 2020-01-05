using System;
using UnityEngine;
using UnityEngine.Networking;

using Logger = PBFramework.Debugging.Logger;

namespace PBFramework.Networking
{
    public class AudioRequest : AssetRequest, IPromise<AudioClip> {

        public event Action<AudioClip> OnFinishedResult
        {
            add => OnFinished += () => value(response.AudioData);
            remove => OnFinished -= () => value(response.AudioData);
        }

        private DownloadHandlerAudioClip downloadHandler;

        private bool isStream;


        public override object Result => response?.AudioData;
        AudioClip IPromise<AudioClip>.Result => response?.AudioData;


        public AudioRequest(string url, bool isStream = false) : base(url)
        {
            this.isStream = isStream;
        }

        protected override void DisposeSoft()
        {
            base.DisposeSoft();
            if (downloadHandler != null)
            {
                downloadHandler = null;
            }
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