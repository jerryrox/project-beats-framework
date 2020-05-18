using UnityEngine;

namespace PBFramework.Assets.Atlasing
{
    /// <summary>
    /// Implementation of IAtlas for loading sprites in resources.
    /// </summary>
    public class ResourceAudioAtlas : ResourceAtlas<AudioClip>
    {
        public override void Set(string name, AudioClip obj) => base.Set(GetAudioPath(name), obj);

        public override AudioClip Get(string name) => base.Get(GetAudioPath(name));

        public override bool Contains(string name) => base.Contains(GetAudioPath(name));

        private string GetAudioPath(string name) => $"Audio/{name}";
    }
}