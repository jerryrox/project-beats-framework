using PBFramework.Data.Bindables;
using PBFramework.Assets.Fonts;

namespace PBFramework.Assets.Fonts
{
    public class FontManager : IFontManager {

        public Bindable<IFont> DefaultFont { get; } = new Bindable<IFont>(null);
    }
}