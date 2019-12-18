using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public interface IDropdown : IGraphicObject, IHasTransition {

        /// <summary>
        /// Event called when the dropdown item has been selected.
        /// </summary>
        event Action<int> OnSelected;


        /// <summary>
        /// Returns the dropdown property container of this dropdown.
        /// </summary>
        IDropdownProperty Property { get; }

        /// <summary>
        /// Returns the background sprite of the list.
        /// </summary>
        ISprite Background { get; }

        /// <summary>
        /// The text label of the list.
        /// </summary>
        ILabel Label { get; }

        /// <summary>
        /// Returns the list of possible options this list provides.
        /// </summary>
        List<Dropdown.OptionData> Options { get; }

        /// <summary>
        /// The index of the current selection.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// The duration in seconds which the dropdown popup fading will animate for.
        /// </summary>
        float FadeDuration { get; set; }


        /// <summary>
        /// Shows the dropdown popup.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the dropdown popup.
        /// </summary>
        void Hide();
    }
}