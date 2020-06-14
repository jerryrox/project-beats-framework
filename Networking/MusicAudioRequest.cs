using PBFramework.Audio;

namespace PBFramework.Networking
{
    public class MusicAudioRequest : ProxyPromise<IMusicAudio> {

        /// <summary>
        /// The inner request being wrapped over.
        /// </summary>
        private AudioRequest audioRequest;


        public MusicAudioRequest(string url, bool isStream = true) : base()
        {
            // Create the actual request to make.
            audioRequest = new AudioRequest(url, isStream);

            // Bind proxied action.
            StartAction = (promise) => audioRequest.Start();
            RevokeAction = audioRequest.Revoke;

            // Hook onto the request.
            audioRequest.OnProgress += SetProgress;
            audioRequest.OnFinishedResult += (audio) => Resolve(new UnityAudio(audio));
        }
    }
}