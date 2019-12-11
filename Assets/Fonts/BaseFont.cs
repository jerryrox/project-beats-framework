using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Assets.Fonts
{
    /// <summary>
    /// Base implementation of IFont.
    /// </summary>
    public abstract class BaseFont : IFont {

        protected Font normalFont;
        protected Font boldFont;
        protected Font italicFont;
        protected Font boldItalicFont;


        public Font Normal => normalFont;

        public Font Bold => boldFont ?? Normal;

        public Font Italic => italicFont ?? Normal;

        public Font BoldItalic => boldItalicFont ?? (boldFont ?? Italic);

        public bool HasBold => boldFont != null;

        public bool HasItalic => italicFont != null;

        public bool HasBoldItalic => boldItalicFont != null;


        public virtual void Dispose() {}
    }
}