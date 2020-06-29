using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Audio
{
    /// <summary>
    /// Audio time synchronizer.
    /// </summary>
    public class AudioClock {

        private IMusicController musicController;

        /// <summary>
        /// The time at which the audio started.
        /// </summary>
        private float startTime = 0f;

        /// <summary>
        /// The time at which the audio paused or stopped.
        /// </summary>
        private float idleTime = 0f;

        /// <summary>
        /// Playback rate.
        /// </summary>
        private float rate = 1f;


        /// <summary>
        /// Returns whether the clock is running.
        /// This flag also indicates whether there is a valid audio clip mounted on the music controller.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Returns whether the audio is playing.
        /// </summary>
        public bool IsPlaying { get; private set; } = false;

        /// <summary>
        /// Returns whether the audio is paused.
        /// </summary>
        public bool IsPaused { get; private set; } = false;

        /// <summary>
        /// Returns whether the audio is stopped.
        /// </summary>
        public bool IsStopped => !IsPlaying && !IsPaused;

        /// <summary>
        /// The rate at which the audio is playing.
        /// </summary>
        public float Rate
        {
            get => rate;
            private set => rate = value;
        }

        /// <summary>
        /// The amount of offset in milliseconds to apply on the clock.
        /// </summary>
        public float Offset { get; set; }

        /// <summary>
        /// Returns the current time relative to audio playback time in milliseconds.
        /// May not return a useful value when not running, not mounted, or not initially played.
        /// </summary>
        public float CurrentTime
        {
        	get
        	{
                float realtime = ToMs(Time.realtimeSinceStartup);
                float time = (realtime - startTime);
        		if(!IsPlaying)
        			time -= realtime - idleTime;
        		return time * rate + Offset;
        	}
        }


        public AudioClock(IMusicController musicController)
        {
            if(musicController == null) throw new ArgumentNullException(nameof(musicController));

            this.musicController = musicController;

            musicController.OnMounted += (audio) => {
                IsRunning = audio != null;
            };
            musicController.OnPlay += (time) => {
                if(IsPlaying)
                    return;
                IsPlaying = true;
                var realtime = ToMs(Time.realtimeSinceStartup);
                startTime = realtime - time;
                idleTime = realtime;
            };
            musicController.OnUnpause += (time) => {
                if(IsPlaying || !IsPaused)
                    return;
                IsPlaying = true;
                IsPaused = false;
                startTime += ToMs(Time.realtimeSinceStartup) - idleTime;
            };
            musicController.OnPause += () => {
                if(!IsPlaying || IsPaused)
                    return;
                IsPlaying = false;
                IsPaused = true;
                idleTime = ToMs(Time.realtimeSinceStartup);
            };
            musicController.OnStop += () => {
                IsPlaying = false;
                IsPaused = false;
                startTime = idleTime = ToMs(Time.realtimeSinceStartup);
            };
            musicController.OnSeek += (time) => {
                var realtime = ToMs(Time.realtimeSinceStartup);
                startTime = realtime - time;
                idleTime = realtime;
            };
            musicController.OnTempo += (tempo) =>
            {
                Rate = tempo;
            };
        }

        /// <summary>
        /// Advances the clock time by specified deltatime.
        /// </summary>
        public void Update(float deltaTime)
        {
        }

        /// <summary>
        /// Converts the specified seconds into milliseconds.
        /// </summary>
        private float ToMs(float seconds) => seconds *= 1000.0f;
    }
}