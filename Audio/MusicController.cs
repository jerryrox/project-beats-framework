using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Audio
{
    public class MusicController : UnityAudioController, IMusicController {

        /// <summary>
        /// Default amount of delay applied when playing the audio controller.
        /// </summary>
        private const float DefaultPlayDelay = 200;

        /// <summary>
        /// Amount of delay to apply when resuming audio after seeking time.
        /// </summary>
        private const float SeekPlayDelay = 50;


        public event Action<float> OnTempo;

        public event Action<float> OnFaded;

        
        /// <summary>
        /// Volume of the audio.
        /// </summary>
        private float volume = 1f;

        /// <summary>
        /// Volume fading scale where the fading effect should start from.
        /// </summary>
        private float fadeScaleFrom = 1f;

        /// <summary>
        /// Volume fading scale where the fading effect should end at.
        /// </summary>
        private float fadeScaleTo = 1f;

        /// <summary>
        /// Current volume fading scale.
        /// </summary>
        private float curFadeScale = 1f;

        private AudioClock clock;


        public override bool IsPlaying => Clock.IsPlaying;

        public override bool IsPaused => Clock.IsPaused;

        public override bool IsStopped => Clock.IsStopped;

        public override float Volume => source.volume;

        public override float CurrentTime => Clock.IsRunning ? Clock.CurrentTime : 0;

        public AudioClock Clock => clock;


        protected override void Awake()
        {
            base.Awake();

            clock = new AudioClock(this);
        }

        /// <summary>
        /// Creates a new music controller instance.
        /// </summary>
        public static MusicController Create()
        {
            GameObject target = GameObject.Find("_MusicController") ?? new GameObject("_MusicController");
            return target.AddComponent<MusicController>();
        }

        public override void Play() => Play(DefaultPlayDelay);

        public override void Seek(float time)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(MusicController));
            if(!Clock.IsRunning)
                return;

			// Store whether it was playing on applying seek.
			bool wasPlaying = Clock.IsPlaying;
			// Stop audio first
			source.Stop();
			// Seek to specified time.
			source.timeSamples = (int)(Audio.Frequency * FromMs(time));
            // Play music again if it was playing before seek.
            if (wasPlaying)
                source.PlayScheduled(AudioSettings.dspTime + FromMs(SeekPlayDelay));
            // Invoke event
            InvokeSeek(time - SeekPlayDelay);
        }

        public override void SetVolume(float volume)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(MusicController));

            this.volume = volume;
            curFadeScale = 0.999f;
        }

        public void Fade(float from, float to)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(MusicController));

            fadeScaleFrom = from;
            fadeScaleTo = to;
            curFadeScale = 0f;
        }

        public void Fade(float to)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(MusicController));
            
            fadeScaleFrom = 0f;
            fadeScaleTo = to;
            curFadeScale = 0f;
        }

        public void SetFade(float fade)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(MusicController));

            fadeScaleFrom = fade;
            fadeScaleTo = fade;
            curFadeScale = 0.999f;
        }

        public void SetTempo(float tempo)
        {
            if(isDisposed) throw new ObjectDisposedException(nameof(MusicController));

            tempo = Math.Max(0.001f, tempo);
            // TODO: Apply on audio mixer
            source.pitch = tempo;
            OnTempo?.Invoke(tempo);
        }

        protected override bool ShouldPlayFromStart() => IsStopped && CurrentTime - Clock.Offset <= 0.00001f;

        protected override bool CanPlay() => base.CanPlay() && Clock.IsRunning;

        protected override bool CanPause() => base.CanPause() && Clock.IsRunning;

        protected override bool CanStop() => base.CanStop() && Clock.IsRunning;

        protected override bool CanSeek() => base.CanSeek() && Clock.IsRunning;

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
            clock = null;
        }

        protected override void Update()
        {
            // Handle audio fade
            if (curFadeScale < 1f)
            {
                // Increase scale
                curFadeScale = Math.Max(1f, curFadeScale + Time.unscaledDeltaTime);
                // Fade volume
                source.volume = volume * Mathf.Lerp(fadeScaleFrom, fadeScaleTo, curFadeScale);
                // Fade finish
                if (curFadeScale >= 1f)
                {
                    OnFaded?.Invoke(fadeScaleTo);
                }
            }

            // Update audio clock
            clock.Update(Time.unscaledDeltaTime);

            // Process audio end and loop only when running and is playing.
            if (Clock.IsRunning && Clock.IsPlaying && Clock.CurrentTime > Audio.Duration)
            {
                base.Update();
            }
        }

        protected override void HandleLoop()
        {
            base.HandleLoop();
            Fade(fadeScaleTo);
        }
    }
}