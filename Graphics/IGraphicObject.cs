using System;
using UnityEngine;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Abstraction of an object that resides in the UI space.
    /// </summary>
    public interface IGraphicObject : IHasTransform, IComparable<IGraphicObject> {

        /// <summary>
        /// Name of the object.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Returns the raw GameObject component under the hood.
        /// </summary>
        GameObject RawObject { get; }

        /// <summary>
        /// Returns the parent object.
        /// </summary>
        IGraphicObject Parent { get; }

        /// <summary>
        /// Sets the rendering depth of the object for comparison with sibling objects.
        /// </summary>
        int Depth { get; set; }

        /// <summary>
        /// (Optional) If implementation supports it, access the unprocessed, raw width value of the transform.
        /// </summary>
        float RawWidth { get; set; }

        /// <summary>
        /// (Optional) If implementation supports it, access the unprocessed, raw height value of the transform.
        /// </summary>
        float RawHeight { get; set; }

        /// <summary>
        /// (Optional) If implementation supports it, access the unprocessed, raw size value of the transform.
        /// </summary>
        Vector2 RawSize { get; set; }


        /// <summary>
        /// Creates a new plain GraphicObject instance under this object and returns it.
        /// </summary>
        IGraphicObject CreateChild(string name = "");

        /// <summary>
        /// Creates a new child with specified component T.
        /// </summary>
        T CreateChild<T>(string name = "") where T : MonoBehaviour, IGraphicObject;

        /// <summary>
        /// Adds the specified type of component while injecting dependencies.
        /// </summary>
        T AddComponentInject<T>() where T : MonoBehaviour;

        /// <summary>
        /// Sets the parent of this object to the specified object.
        /// </summary>
        void SetParent(IGraphicObject parent);

        /// <summary>
        /// Destroys this object.
        /// </summary>
        void Destroy();
    }
}