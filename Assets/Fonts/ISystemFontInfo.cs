namespace PBFramework.Assets.Fonts
{
    public interface ISystemFontInfo {
    
        /// <summary>
        /// Returns the name of the normal font variant.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the name of the bold font variant.
        /// </summary>
        string BoldName { get; }

        /// <summary>
        /// Returns the name of the italic font variant.
        /// </summary>
        string ItalicName { get; }

        /// <summary>
        /// Returns the name of the bold italic font variant.
        /// </summary>
        string BoldItalicName { get; }
    }
}