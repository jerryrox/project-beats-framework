using System.Text;

namespace PBFramework.Assets.Fonts
{
    /// <summary>
    /// Information of a system font.
    /// </summary>
    public class SystemFontInfo : ISystemFontInfo {

        public string Name { get; set; }

        public string BoldName { get; set; }

        public string ItalicName { get; set; }

        public string BoldItalicName { get; set; }


        public IFont ToFont() => new SystemFont(this);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Font name: {Name}");
            if(!string.IsNullOrEmpty(BoldName))
                sb.AppendLine($"- Bold variant: {BoldName}");
            if(!string.IsNullOrEmpty(ItalicName))
                sb.AppendLine($"- Italic variant: {ItalicName}");
            if(!string.IsNullOrEmpty(BoldItalicName))
                sb.AppendLine($"- Bold Italic variant: {BoldItalicName}");
            return sb.ToString();
        }
    }
}