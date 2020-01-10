using UnityEngine;

namespace PBFramework.Assets.Fonts
{
    /// <summary>
    /// Font variant which assumes font location in the resources folder.
    /// </summary>
    public class ResourceFont : BaseFont {

        private bool unloadOnDispose;


        public ResourceFont(string resourcePath, bool unloadOnDispose = true)
        {
            this.unloadOnDispose = unloadOnDispose;

            normalFont = Resources.Load(resourcePath, typeof(Font)) as Font;
            boldFont = Resources.Load($"{resourcePath}-Bold", typeof(Font)) as Font;
            italicFont = Resources.Load($"{resourcePath}-Italic", typeof(Font)) as Font;
            boldItalicFont = Resources.Load($"{resourcePath}-BoldItalic", typeof(Font)) as Font;
        }

        public override void Dispose()
        {
            if (unloadOnDispose)
            {
                if(normalFont != null)
                    Resources.UnloadAsset(normalFont);
                if(boldFont != null)
                    Resources.UnloadAsset(boldFont);
                if(italicFont != null)
                    Resources.UnloadAsset(italicFont);
                if(boldItalicFont != null)
                    Resources.UnloadAsset(boldItalicFont);
            }
        }
    }
}