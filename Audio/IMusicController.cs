using System;

namespace PBFramework.Audio
{
    /// <summary>
    /// Interface for controlling musics.
    /// </summary>
    public interface IMusicController : IAudioController {

        /// <summary>
        /// Event called when the controller has changed the playback tempo.
        /// </summary>
        event Action<float> OnTempo;

        /// <summary>
        /// Event called when the volume fading has finished.
        /// </summary>
        event Action<float> OnFaded;


        /// <summary>
        /// Returns the clock for this controller.
        /// </summary>
        AudioClock Clock { get; }


        /// <summary>
        /// Fades audio volume between specified range.
        /// </summary>
        void Fade(float from, float to);

        /// <summary>
        /// Fades audio from scale 0 to specified scale.
        /// </summary>
        void Fade(float to);

        /// <summary>
        /// Sets audio fade scale to specified value.
        /// </summary>
        void SetFade(float fade);

        /// <summary>
        /// Sets audio playback tempo.
        /// </summary>
        void SetTempo(float tempo);
    }
}