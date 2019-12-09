using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBFramework.Graphics.UI
{
    public class UguiSprite : UguiObject<Image>, ISprite {
    
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

        public Sprite Sprite
        {
            get => component.sprite;
            set => component.sprite = value;
        }

        public Material Material
        {
            get => component.material;
            set => component.material = value;
        }

        public Image.Type ImageType
        {
            get => component.type;
            set => component.type = value;
        }

        public float FillAmount
        {
            get => component.fillAmount;
            set => component.fillAmount = value;
        }

        public bool FillInverse
        {
            get => !component.fillClockwise;
            set => component.fillClockwise = !value;
        }


        public override void ResetSize()
        {
            component.SetNativeSize();
        }

        public void SetRadial360Fill(Image.Origin360 origin)
        {
            component.fillMethod = Image.FillMethod.Radial360;
            component.fillOrigin = (int)origin;
        }

        public void SetRadial180Fill(Image.Origin180 origin)
        {
            component.fillMethod = Image.FillMethod.Radial180;
            component.fillOrigin = (int)origin;
        }

        public void SetRadial90Fill(Image.Origin90 origin)
        {
            component.fillMethod = Image.FillMethod.Radial90;
            component.fillOrigin = (int)origin;
        }

        public void SetHorizontalFill(Image.OriginHorizontal origin)
        {
            component.fillMethod = Image.FillMethod.Horizontal;
            component.fillOrigin = (int)origin;
        }

        public void SetVerticalFill(Image.OriginVertical origin)
        {
            component.fillMethod = Image.FillMethod.Vertical;
            component.fillOrigin = (int)origin;
        }
    }
}