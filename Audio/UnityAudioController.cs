using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Audio
{
    /// <summary>
    /// Base implementation of other IAudioControllers for Unity audio sources.
    /// </summary>
    public abstract class UnityAudioController : MonoBehaviour, IAudioController {

        public event Action<IAudio> OnMounted;

        public event Action<float> OnPlay;

        public event Action<float> OnUnpause;

        public event Action OnPause;

        public event Action OnStop;

        public event Action<float> OnSeek;

        public event Action OnEnd;

        public event Action OnLoop;
        

        /// <summary>
        /// Internal audio player component.
        /// </summary>
        protected AudioSource source;

        /// <summary>
        /// Whether the controller is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// The actual mounted audio on this controller.
        /// </summary>
        protected new UnityAudio audio;


        public IAudio Audio => audio;

        public abstract bool IsPlaying { get; }

        public abstract bool IsPaused { get; }

        public abstract bool IsStopped { get; }

        public virtual float Volume => source.volume;

        public float LoopTime { get; set; } = 0f;

        public virtual float CurrentTime => ToMs(source.time);

        public virtual float Progress => audio == null ? 0f : CurrentTime / audio.Duration;

        public bool IsLoop { get; set; } = false;


        protected virtual void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
        }

        public void MountAudio(IAudio audio)
        {
            if(isDisposed) throw new ObjectDisposedException(GetType().Name);
            
            // Unmount audio.
            if (audio == null)
            {
                this.audio = null;
                source.clip = null;
                return;
            }

            // Check if compatible type with this controller.
            if (!(audio is UnityAudio newAudio))
                throw new ArgumentException($"Specified audio is not a type of ({nameof(UnityAudio)}). Given type: {audio.GetType().Name}");

            // Mount the audio.
            this.audio = newAudio;
            source.clip = newAudio.Clip;
            InvokeMounted(audio);
        }

        public virtual void Play() => Play(0);

        public virtual void Play(float delay)
        {
            if(isDisposed) throw new ObjectDisposedException(GetType().Name);
            if(!CanPlay())
                return;

            if(ShouldPlayFromStart())
            {
                delay = Math.Max(0, delay);

                double playTime = AudioSettings.dspTime + FromMs(delay);
                source.timeSamples = 0;
                source.PlayScheduled(playTime);
                InvokePlay(-delay);
            }
            else
            {
                // Even if the controller is stopped, the above if statement may not be executed due to additional conditions.
                // In case the controller is currently not paused, make it paused first.
                if (!IsPaused)
                    source.Pause();
                source.UnPause();
                InvokeUnpause(ToMs(source.time));
            }
        }

        public virtual void Pause()
        {
            if(isDisposed) throw new ObjectDisposedException(GetType().Name);
            if(!CanPause())
                return;

            source.Pause();
            InvokePause();
        }

        public virtual void Stop()
        {
            if(isDisposed) throw new ObjectDisposedException(GetType().Name);
            if(!CanStop())
                return;

            source.Stop();
            InvokeStop();
        }

        public virtual void Seek(float time)
        {
            if(isDisposed) throw new ObjectDisposedException(GetType().Name);
            if(!CanSeek())
                return;

            // Seek to specified time.
            source.timeSamples = (int)(audio.Frequency * FromMs(time));
            InvokeSeek(time);
        }

        public virtual void SetVolume(float volume)
        {
            if(isDisposed) throw new ObjectDisposedException(GetType().Name);

            source.volume = volume;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                DisposeInternal();
            }
            isDisposed = true;
        }

        /// <summary>
        /// Returns whether Play() should play the audio from start or unpause.
        /// </summary>
        protected virtual bool ShouldPlayFromStart() => IsStopped && CurrentTime <= 0f;

        protected virtual bool CanPlay() => !IsPlaying;

        protected virtual bool CanPause() => !IsPaused;

        protected virtual bool CanStop() => !IsStopped;

        protected virtual bool CanSeek() => true;

        protected virtual void DisposeInternal()
        {
            source.clip = null;
            UnityEngine.Object.Destroy(source);
            source = null;

            audio = null;

            enabled = false;
        }

        protected virtual void Update()
        {
            // Handle audio end and loop.
            if (audio != null && CurrentTime >= audio.Duration)
            {
                // If automatially loop
                if(IsLoop)
                {
                    // Handle looping action.
                    HandleLoop();

                    // Seek to loop time, as long as it's valid.
                    if(LoopTime >= 0 && LoopTime < audio.Duration)
                        Seek(LoopTime);

                    // Invoke loop event
                    InvokeLoop();
                }
                else
                {
                    // Stop the audio.
                    Stop();

                    // Invoke end event
                    InvokeEnd();
                }
            }
        }

        protected virtual void HandleLoop()
        {
            Stop();
            Play();
        }

        protected void InvokeMounted(IAudio audio) => OnMounted?.Invoke(audio);

        protected void InvokePlay(float time) => OnPlay?.Invoke(time);

        protected void InvokeUnpause(float time) => OnUnpause?.Invoke(time);

        protected void InvokePause() => OnPause?.Invoke();

        protected void InvokeStop() => OnStop?.Invoke();

        protected void InvokeSeek(float time) => OnSeek?.Invoke(time);

        protected void InvokeEnd() => OnEnd?.Invoke();

        protected void InvokeLoop() => OnLoop?.Invoke();

        protected float ToMs(float time) => time * 1000.0f;

        protected float FromMs(float ms) => ms * 0.001f;
    }
}