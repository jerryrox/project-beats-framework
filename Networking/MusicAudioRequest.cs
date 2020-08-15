using PBFramework.Audio;
using PBFramework.Threading.Futures;
using UnityEngine;

namespace PBFramework.Networking
{
    public class MusicAudioRequest : ProxyFuture<AudioClip, IMusicAudio> {

        public MusicAudioRequest(string url, bool isStream = true) : base(new AudioRequest(url, isStream))
        {
        }

        protected override IMusicAudio ConvertOutput(AudioClip source)
        {
            return new UnityAudio(source);
        }
    }
}