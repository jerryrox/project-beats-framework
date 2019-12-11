using System;
using UnityEngine;

namespace PBFramework.Assets.Fonts
{
    /// <summary>
    /// Abstraction of Unity font which includes multiple similar fonts to provide fallbacks and style variants.
    /// </summary>
    public interface IFont : IDisposable {

        /// <summary>
        /// Returns the font of normal style.
        /// </summary>
        Font Normal { get; }

        /// <summary>
        /// Returns the font of bold style.
        /// If unavailable, Normal font will be returned.
        /// </summary>
        Font Bold { get; }

        /// <summary>
        /// Returns the font of italic style.
        /// If unavailable, Normal font will be returned.
        /// </summary>
        Font Italic { get; }

        /// <summary>
        /// Returns the font of bold and italic style.
        /// If unavailable, Bold or Italic or Normal font will be returned, based on availability.
        /// </summary>
        Font BoldItalic { get; }

        /// <summary>
        /// Returns whether the bold font variant exists.
        /// </summary>
        bool HasBold { get; }

        /// <summary>
        /// Returns whether the italic font variant exists.
        /// </summary>
        bool HasItalic { get; }

        /// <summary>
        /// Returns whether the bold & italic font variant exists.
        /// </summary>
        bool HasBoldItalic { get; }
    }
}