using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public class UguiTexture : UguiObject<RawImage>, ITexture {

        public Material Material
        {
            get => component.material;
            set => component.material = value;
        }

        public float Alpha
        {
            get => component.color.a;
            set => component.SetAlpha(value);
        }

        public Color Color
        {
            get => component.color;
            set => component.color = value;
        }

        public Texture Texture
        {
            get => component.texture;
            set => component.texture = value;
        }

        public Rect UVRect
        {
            get => component.uvRect;
            set => component.uvRect = value;
        }


        public void FillTexture()
        {
            var texture = component.texture;
            if(texture == null) return;

            float textureRatio = (float)texture.width / (float)texture.height;
            float widgetRatio = Width / Height;

            // If the texture is vertically longer in terms of widget ratio
            if(textureRatio > widgetRatio)
            {
                // Fit texture to height.
                float sizeX = widgetRatio / textureRatio;
                float offsetX = (1f - sizeX) / 2f;
                UVRect = new Rect(offsetX, 0f, sizeX, 1f);
            }
            else
            {
                // Fit texture to width.
                float sizeY = textureRatio / widgetRatio;
                float offsetY = (1f - sizeY) / 2f;
                UVRect = new Rect(0f, offsetY, 1f, sizeY);
            }
        }
    }
}