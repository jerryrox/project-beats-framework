using System;
using System.Collections;
using System.Collections.Generic;
using PBFramework.Debugging;

namespace PBFramework.Graphics.UI.Elements.NGUI
{
    public class NguiPanel : NguiElement<UIPanel>, IPanel {
    
        public int Depth
        {
            get => component.depth;
            set => component.depth = value;
        }

        public float Alpha
        {
            get => component.alpha;
            set => component.alpha = value;
        }

        public float Width
        {
            get => component.width;
            set
            {
                var region = component.finalClipRegion;
                component.SetRect(region.x, region.y, value, region.w);
            }
        }

        public float Height
        {
            get => component.height;
            set
            {
                var region = component.finalClipRegion;
                component.SetRect(region.x, region.y, region.z, value);
            }
        }

        public Pivots Pivot
        {
            get => Pivots.Center;
            set => Logger.Log("NguiPanel.Pivot - Unsupported setter.");
        }

        public ClipModes ClipMode
        {
            get => NguiGraphicsHelper.NguiToGraphicsClipping(component.clipping);
            set => component.clipping = NguiGraphicsHelper.GraphicsToNguiClipping(value);
        }


        public void ResetSize() => Logger.Log("NguiPanel.ResetSize - Unsupported method.");
    }
}