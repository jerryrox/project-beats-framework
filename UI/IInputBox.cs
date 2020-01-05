using System;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Graphics;

namespace PBFramework.UI
{
    public interface IInputBox : IGraphicObject, IHasTransition, ILabel {

        /// <summary>
        /// Event called when the input value has been changed.
        /// </summary>
        event Action<string> OnChanged;

        /// <summary>
        /// Event called when the input value has been finalized.
        /// </summary>
        event Action<string> OnSubmitted;


        /// <summary>
        /// Returns the label instance being used for displaying the placeholder text.
        /// </summary>
        ILabel PlaceholderLabel { get; }

        /// <summary>
        /// Returns the label instance being used for displaying the input box value.
        /// </summary>
        ILabel ValueLabel { get; }

        /// <summary>
        /// Returns the background sprite displayer.
        /// </summary>
        ISprite Background { get; }

        /// <summary>
        /// The default text to be displayed when the input text is empty.
        /// </summary>
        string Placeholder { get; set; }

        /// <summary>
        /// Max number of characters limited to enter.
        /// </summary>
        int CharacterLimit { get; set; }

        /// <summary>
        /// Color of the text selection area.
        /// </summary>
        Color SelectionColor { get; set; }

        /// <summary>
        /// Type of line modes of the input.
        /// Default: SingleLine
        /// </summary>
        InputField.LineType LineType { get; set; }

        /// <summary>
        /// Type of input mode of the input.
        /// Default: Standard
        /// </summary>
        InputField.InputType InputType { get; set; }

        /// <summary>
        /// Type of keyboard to show on touchscreens.
        /// Default: Default
        /// </summary>
        TouchScreenKeyboardType KeyboardType { get; set; }

        /// <summary>
        /// Type of text validation to use for the input.
        /// Default: None
        /// </summary>
        InputField.CharacterValidation ValidationType { get; set; }
    }
}