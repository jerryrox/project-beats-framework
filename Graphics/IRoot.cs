using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Abstraction of a root object of the UI hierarchy tree.
    /// </summary>
    public interface IRoot : IGraphicObject {

        /// <summary>
        /// Returns the actual resolution of the root.
        /// </summary>
        Vector2 Resolution { get; }

        /// <summary>
        /// The base resolution of the root.
        /// </summary>
        Vector2 BaseResolution { get; set; }

        /// <summary>
        /// Returns the camera associated with the root, if using camera render.
        /// </summary>
        Camera Cam { get; }


        /// <summary>
        /// Sets rendering mode to overlay.
        /// </summary>
        void SetOverlayRender(int sortOrder = 0);

        /// <summary>
        /// Sets rendering mode to camera.
        /// </summary>
        void SetCameraRender(Camera camera);
    }
}