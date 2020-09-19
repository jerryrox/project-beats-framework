using PBFramework.Audio;

namespace PBFramework.Networking
{
    public class MusicAudioRequest : WrappedWebRequest<AudioRequest, IMusicAudio> {

        public MusicAudioRequest(string url, bool isStream = true) :
            base(new AudioRequest(url, isStream))
        { }

        protected override IMusicAudio GetOutput(AudioRequest request)
        {
            return new UnityAudio(request.Output);
        }
    }
}