using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Assets.Atlasing
{
    /// <summary>
    /// Implementation of IAtlas for loading sprites in resources.
    /// </summary>
    public class ResourceSpriteAtlas : ResourceAtlas<Sprite> {

        public override void Set(string name, Sprite obj) => base.Set(GetSpritePath(name), obj);

        public override Sprite Get(string name) => base.Get(GetSpritePath(name));

        public override bool Contains(string name) => base.Contains(GetSpritePath(name));

        private string GetSpritePath(string name) => $"Sprites/{name}";
    }
}