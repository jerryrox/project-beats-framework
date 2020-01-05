using UnityEngine;

namespace PBFramework.Graphics
{
    public interface IHasMaterial {
    
        /// <summary>
        /// The material to be displayed on the object.
        /// </summary>
        Material Material { get; set; }
    }
}