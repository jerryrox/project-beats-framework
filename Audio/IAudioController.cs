using System;

namespace PBFramework.Audio
{
    /// <summary>
    /// Shared interface for music or effect type controllers.
    /// </summary>
    public interface IAudioController : IDisposable {
    
        /// <summary>
        /// Event called when a new audio has been mounted on the controller.
        /// </summary>
        event Action<IAudio> OnMounted;

        /// <summary>
        /// Event called when the controller has started playing the audio from start.
        /// Returns the current time in milliseconds.
        /// </summary>
        event Action<float> OnPlay;

        /// <summary>
        /// Event called when the controller has unpaused the audio.
        /// Returns the current time in milliseconds.
        /// </summary>
        event Action<float> OnUnpause;

        /// <summary>
        /// Event called when the controller has paused the audio.
        /// </summary>
        event Action OnPause;

        /// <summary>
        /// Event called when the controller has stopped the audio.
        /// </summary>
        event Action OnStop;

        /// <summary>
        /// Event called when the controller has sought to a new time.
        /// Returns the current time in milliseconds.
        /// </summary>
        event Action<float> OnSeek;

        /// <summary>
        /// Event called when the controller has reached the audio playback end.
        /// </summary>
        event Action OnEnd;

        /// <summary>
        /// Event called when the controller has started playing the audio again from beginning due to loop.
        /// </summary>
        event Action OnLoop;


        /// <summary>
        /// Returns the audio currently being played.
        /// </summary>
        IAudio Audio { get; }

        /// <summary>
        /// Returns whether the controller is playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Returns whether the controller is paused.
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Returns whether the controller is stopped.
        /// </summary>
        bool IsStopped { get; }

        /// <summary>
        /// Returns current volume of the controller.
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// The time in milliseconds when the looped audio should replay from.
        /// </summary>
        float LoopTime { get; set; }

        /// <summary>
        /// Returns the current playback time in milliseconds.
        /// </summary>
        float CurrentTime { get; }

        /// <summary>
        /// Returns the current playback progress.
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Whether the controller should loop the audio playback on reaching the end.
        /// </summary>
        bool IsLoop { get; set; }


        /// <summary>
        /// Mounts the specified audio on the controller for playback.
        /// </summary>
        void MountAudio(IAudio audio);

        /// <summary>
        /// Starts the audio playback from start if stopped.
        /// If paused, audio will be unpaused.
        /// </summary>
        void Play();

        /// <summary>
        /// Starts the audio playback after specified milliseconds.
        /// If paused, audio will be unpaused immediately.
        /// </summary>
        void Play(float delay);

        /// <summary>
        /// Pauses the audio playback.
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops the audio playback and resets time to 0.
        /// </summary>
        void Stop();

        /// <summary>
        /// Seeks current playback time to specified value in milliseconds.
        /// </summary>
        void Seek(float time);

        /// <summary>
        /// Sets current playback volume to specified value.
        /// </summary>
        void SetVolume(float volume);
    }
}