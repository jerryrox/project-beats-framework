using System;
using UnityEngine;
using PBFramework.Animations.Sections;

namespace PBFramework.Animations
{
    /// <summary>
    /// Delegate for handling animations.
    /// </summary>
    public delegate void AnimateHandler<T>(T value);

    public interface IAnime {

        /// <summary>
        /// Returns whether the animation is currently playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Returns current time in seconds.
        /// </summary>
        float Time { get; }

        /// <summary>
        /// Returns the total duration of the animation per cycle.
        /// </summary>
        float Duration { get; }

        /// <summary>
        /// The playback speed of the animation.
        /// Default: 1
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// Type of wrap mode applied on the animation.
        /// Default: None
        /// </summary>
        WrapModeType WrapMode { get; set; }

        /// <summary>
        /// Type of stop mode applied on the animation.
        /// Default: Reset
        /// </summary>
        StopModeType StopMode { get; set; }


        /// <summary>
        /// Creates a new event at specified time.
        /// </summary>
        void AddEvent(float time, Action action);

        /// <summary>
        /// Creates a new section which animates Vector2 value.
        /// </summary>
        ISection<Vector2> AnimateVector2(AnimateHandler<Vector2> handler);
        
        /// <summary>
        /// Creates a new section which animates Vector3 value.
        /// </summary>
        ISection<Vector3> AnimateVector3(AnimateHandler<Vector3> handler);
        
        /// <summary>
        /// Creates a new section which animates Color value.
        /// </summary>
        ISection<Color> AnimateColor(AnimateHandler<Color> handler);

        /// <summary>
        /// Creates a new section which animates float value.
        /// </summary>
        ISection<float> AnimateFloat(AnimateHandler<float> handler);

        /// <summary>
        /// Creates a new section which animates int value.
        /// </summary>
        ISection<int> AnimateInt(AnimateHandler<int> handler);

        /// <summary>
        /// Starts playing the animation from current position.
        /// </summary>
        void Play();

        /// <summary>
        /// Starts playing the animation from the beginning.
        /// </summary>
        void PlayFromStart();

        /// <summary>
        /// Pauses the playback at current position.
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops the playback and resets current position based on current StopMode.
        /// </summary>
        void Stop();

        /// <summary>
        /// Changes current position to specified time.
        /// </summary>
        void Seek(float time);

        /// <summary>
        /// Updates the animation by specified time fragment.
        /// Returns whether the animation is still playing.
        /// </summary>
        bool Update(float deltaTime);
    }
}