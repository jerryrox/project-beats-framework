using System;
using System.Collections.Generic;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Indicates that the object receives layered input management service from the InputManager.
    /// The CompareTo method should make sure the receivers are sorted in a descending order.
    /// </summary>
    public interface IInputReceiver : IComparable<IInputReceiver> {

        /// <summary>
        /// Returns the layer at which the input receiving should work on.
        /// Higher value should indicate higher priority.
        /// </summary>
        int InputLayer { get; }


        /// <summary>
        /// Prepares this object for input receiver sorting in the manager.
        /// </summary>
        void PrepareInputSort();

        /// <summary>
        /// Processes input event.
        /// Returns whether input can be propagated to lower-layered objects.
        /// </summary>
        bool ProcessInput();
    }
}