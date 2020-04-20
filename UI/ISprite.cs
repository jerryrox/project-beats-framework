using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Assets.Atlasing;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface ISprite : IGraphicObject, IHasColor, IHasTint, IHasMaterial, IHasFill {

        /// <summary>
        /// Event called when the rendered sprite has changed.
        /// </summary>
        event Action<Sprite> OnSpriteChange;


        /// <summary>
        /// The atlas which the sprites are retrieved from.
        /// </summary>
        IAtlas<Sprite> Atlas { get; set; }

        /// <summary>
        /// The sprite to be displayed on the object.
        /// </summary>
        Sprite Sprite { get; set; }

        /// <summary>
        /// The name of the sprite to be displayed on the object.
        /// /// This can be used instead of assigning Sprite property.
        /// </summary>
        string SpriteName { get; set; }

        /// <summary>
        /// Type of image displaying method.
        /// </summary>
        Image.Type ImageType { get; set; }
    }
}