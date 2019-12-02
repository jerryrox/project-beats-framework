using UnityEngine;

namespace PBFramework.Audio
{
    public class UnityAudio : IEffectAudio, IMusicAudio {

        public float Duration => ToMs(Clip.length);

        public int Frequency => Clip.frequency;

        public int Channels => Clip.channels;

        public AudioClip Clip { get; private set; }


        public UnityAudio(AudioClip clip)
        {
            Clip = clip;
        }

        public void Dispose()
        {
            if (Clip != null)
            {
                Object.Destroy(Clip);
                Clip = null;
            }
        }

        private float ToMs(float time) => time * 1000.0f;
    }
}