using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Audio
{
    public class EffectController : UnityAudioController, IEffectController
    {

        /// <summary>
        /// Whether the effect is currently playing.
        /// </summary>
        private bool isPlaying;

        /// <summary>
        /// Whether the effect is currently paused.
        /// </summary>
        private bool isPaused;


        public override bool IsPlaying => isPlaying;

        public override bool IsPaused => isPaused;

        public override bool IsStopped => !isPlaying && !isPaused;


        public override void Play(float delay)
        {
            base.Play(delay);
            if (CanPlay())
            {
                isPlaying = true;
                isPaused = false;
            }
        }

        public override void Pause()
        {
            base.Pause();
            if (CanPause())
            {
                isPlaying = false;
                isPaused = true;
            }
        }

        public override void Stop()
        {
            base.Stop();
            if (CanStop())
            {
                isPlaying = false;
                isPaused = false;
            }
        }
    }
}