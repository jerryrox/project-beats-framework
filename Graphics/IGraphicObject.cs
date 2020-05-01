using System;
using UnityEngine;
using PBFramework.Inputs;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Abstraction of an object that resides in the UI space.
    /// </summary>
    public interface IGraphicObject : IHasTransform, IHasOffset, IHasEffect, IInputReceiver, IComparable<IGraphicObject> {

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
        /// The dependency container used by this graphic object to use and pass down the hierarchy.
        /// </summary>
        IDependencyContainer Dependencies { get; set; }

        /// <summary>
        /// Whether the host object is currently active.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Sets the rendering depth of the object for comparison with sibling objects.
        /// </summary>
        int Depth { get; set; }

        /// <summary>
        /// Returns the number of direct children in this object.
        /// </summary>
        int ChildCount { get; }

        /// <summary>
        /// If implementation supports it, access the unprocessed, raw width value of the transform.
        /// </summary>
        float RawWidth { get; set; }

        /// <summary>
        /// If implementation supports it, access the unprocessed, raw height value of the transform.
        /// </summary>
        float RawHeight { get; set; }

        /// <summary>
        /// If implementation supports it, access the unprocessed, raw size value of the transform.
        /// </summary>
        Vector2 RawSize { get; set; }


        /// <summary>
        /// Sets whether this object should listen to inputs.
        /// </summary>
        void SetReceiveInputs(bool listen);
        
        /// <summary>
        /// Creates a new plain GraphicObject instance under this object and returns it.
        /// </summary>
        IGraphicObject CreateChild(string name = "", int depth = 0, IDependencyContainer dependencies = null);

        /// <summary>
        /// Creates a new child with specified component T.
        /// </summary>
        T CreateChild<T>(string name = "", int depth = 0, IDependencyContainer dependencies = null) where T : MonoBehaviour, IGraphicObject;

        /// <summary>
        /// Adds the specified type of component while injecting dependencies.
        /// </summary>
        T AddComponentInject<T>() where T : MonoBehaviour;

        /// <summary>
        /// Sets the parent of this object to the specified object.
        /// </summary>
        void SetParent(IGraphicObject parent);

        /// <summary>
        /// Returns the position relative to this object at specified corner.
        /// </summary>
        Vector2 GetPositionAtCorner(Pivots corner, Space space = Space.Self);

        /// <summary>
        /// Invokes the specified action after specified number of frames.
        /// </summary>
        void InvokeAfterFrames(int frames, Action action);

        /// <summary>
        /// Invokes the specified action after a transformation has been applied.
        /// This method should be used in cases where the object's rect depends on anchor to adjust to parent object first,
        /// but is unknown exactly when this will be applied.
        /// You can specify how many frames to wait at most before expecting invocation.
        /// </summary>
        void InvokeAfterTransformed(int maxFrames, Action action);

        /// <summary>
        /// Destroys this object.
        /// </summary>
        void Destroy();
    }
}