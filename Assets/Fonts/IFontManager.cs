using PBFramework.Data.Bindables;
using PBFramework.Assets.Fonts;

namespace PBFramework.Assets.Fonts
{
    public interface IFontManager {
    
        /// <summary>
        /// The default font to be used within the framework/app context.
        /// </summary>
        Bindable<IFont> DefaultFont { get; }
    }
}