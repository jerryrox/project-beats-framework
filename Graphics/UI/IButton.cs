﻿using System;
using UnityEngine;

namespace PBFramework.Graphics.UI
{
    public interface IButton : IGraphicObject, IHasTransition {

        /// <summary>
        /// Event called when the button has been clicked.
        /// </summary>
        event Action OnClick;
        

        /// <summary>
        /// Returns the label component of the button.
        /// </summary>
        ILabel Label { get; }

        /// <summary>
        /// Returns the background sprite of the button.
        /// </summary>
        ISprite Background { get; }

    }
}