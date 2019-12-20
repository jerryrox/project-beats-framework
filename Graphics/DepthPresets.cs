using System;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Graphics
{
    /// <summary>
    /// Provides presets of depth values.
    /// All the preset values may be modified freely depending on the design of the game.
    /// </summary>
    public static class DepthPresets {

        /// <summary>
        /// Depth of the dropdown popup.
        /// </summary>
        public static int DropdownPopup = 100001;

        /// <summary>
        /// Depth of the block for popups.
        /// </summary>
        public static int PopupBlocker = 100000;
    }
}