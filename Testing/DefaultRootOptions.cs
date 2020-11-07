#if UNITY_EDITOR
using UnityEngine;

namespace PBFramework.Testing
{
    public class DefaultRootOptions {

        /// <summary>
        /// The base resolution of the root.
        /// </summary>
        public Vector2 BaseResolution { get; set; } = new Vector2(1280f, 720f);
    }
}
#endif