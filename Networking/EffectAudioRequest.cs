using PBFramework.Audio;

namespace PBFramework.Networking
{
    /// <summary>
    /// A request wrapper over audio request with ITask as base interface to retrieve effect audio.
    /// </summary>
    public class EffectAudioRequest : WrappedWebRequest<AudioRequest, IEffectAudio> {

        public EffectAudioRequest(string url, bool isStream = false) :
            base(new AudioRequest(url, isStream))
        { }

        protected override IEffectAudio GetOutput(AudioRequest request)
        {
            return new UnityAudio(request.Output);
        }
    }
}