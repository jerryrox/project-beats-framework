using PBFramework.Audio;
using PBFramework.Threading.Futures;
using UnityEngine;

namespace PBFramework.Networking
{
    public class EffectAudioRequest : ProxyFuture<AudioClip, IEffectAudio> {

        public EffectAudioRequest(string url, bool isStream = false) : base(new AudioRequest(url, isStream))
        {
        }

        protected override IEffectAudio ConvertOutput(AudioClip source)
        {
            return new UnityAudio(source);
        }
    }
}