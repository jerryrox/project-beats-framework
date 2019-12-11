using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Assets.Fonts
{
    /// <summary>
    /// BaseFont extension for system fonts.
    /// </summary>
    public class SystemFont : BaseFont
    {
        private ISystemFontInfo fontInfo;


        public SystemFont(ISystemFontInfo fontInfo)
        {
            if(!string.IsNullOrEmpty(fontInfo.Name))
                normalFont = Font.CreateDynamicFontFromOSFont(fontInfo.Name, 20);
            if(!string.IsNullOrEmpty(fontInfo.BoldName))
                boldFont = Font.CreateDynamicFontFromOSFont(fontInfo.BoldName, 20);
            if(!string.IsNullOrEmpty(fontInfo.ItalicName))
                italicFont = Font.CreateDynamicFontFromOSFont(fontInfo.ItalicName, 20);
            if(!string.IsNullOrEmpty(fontInfo.BoldItalicName))
                boldItalicFont = Font.CreateDynamicFontFromOSFont(fontInfo.BoldItalicName, 20);
        }

        public override void Dispose()
        {
            if(normalFont != null)
                Object.Destroy(normalFont);
            if(boldFont != null)
                Object.Destroy(boldFont);
            if(italicFont != null)
                Object.Destroy(italicFont);
            if(boldItalicFont != null)
                Object.Destroy(boldItalicFont);

            normalFont = null;
            boldFont = null;
            italicFont = null;
            boldItalicFont = null;
        }
    }
}