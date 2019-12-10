using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Abstraction of a root object of the UI hierarchy tree.
    /// </summary>
    public interface IRoot : IGraphicObject {

        /// <summary>
        /// Resolution of the UI canvas.
        /// </summary>
        Vector2 Resolution { get; set; }


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