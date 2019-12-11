using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Assets.Fonts
{
    public static class SystemFontProvider {

        /// <summary>
        /// List of all informations about system fonts.
        /// </summary>
        public static IReadOnlyList<ISystemFontInfo> Fonts { get; private set; }


        static SystemFontProvider()
        {
            FindAllFonts();
        }

        /// <summary>
        /// Finds all fonts available in OS and parses as font info.
        /// </summary>
        private static void FindAllFonts()
        {
            List<ISystemFontInfo> fontInfos = new List<ISystemFontInfo>();

            // Start parsing available fonts.
            var fonts = Font.GetOSInstalledFontNames();
            SystemFontInfo curInfo = null;
            foreach (var font in fonts)
            {
                if (curInfo != null)
                {
                    // Make sure it's the same family.
                    if (IsSameFamily(font, curInfo.Name))
                    {
                        // If any of the variants, assign it.
                        if(IsBoldVariant(font))
                            curInfo.BoldName = font;
                        else if(IsBoldItalicVariant(font))
                            curInfo.BoldItalicName = font;
                        else if(IsItalicVariant(font))
                            curInfo.ItalicName = font;
                        // If somehow the same family but does not have a variant, treat this as a new.
                        else
                            curInfo = null;
                    }
                    // Else, it should be a new font family.
                    else
                        curInfo = null;
                }
                if (curInfo == null)
                {
                    // If the name indicates a variant, just ignore this font family.
                    if(IsBoldVariant(font) || IsItalicVariant(font))
                        continue;

                    // Else, register a new system font info.
                    curInfo = new SystemFontInfo()
                    {
                        Name = font
                    };
                    fontInfos.Add(curInfo);
                }
            }

            // Store the font info list.
            Fonts = fontInfos;
        }

        private static bool IsSameFamily(string name, string baseName)
        {
            return name.StartsWith(baseName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns whether the specified name indicates a bold font variant.
        /// </summary>
        private static bool IsBoldVariant(string name)
        {
            return name.EndsWith("Bold", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns whether the specified name indicates an italic font variant.
        /// </summary>
        private static bool IsItalicVariant(string name)
        {
            return name.EndsWith("Italic", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns whether the specified name indicates a bold & italic font variant.
        /// </summary>
        private static bool IsBoldItalicVariant(string name)
        {
            return name.EndsWith("Bold Italic", StringComparison.OrdinalIgnoreCase);
        }
    }
}