using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    public class UguiRoot : UguiObject, IRoot {

        private Canvas canvas;
        private CanvasScaler scaler;
        private GraphicRaycaster raycaster;


        public Vector2 Resolution
        {
            get => scaler.referenceResolution;
            set => scaler.referenceResolution = value;
        }


        /// <summary>
        /// Creates a new instance of the UguiRoot and returns it.
        /// Optionally provide a dependency container to pass down dependencies.
        /// </summary>
        public static UguiRoot Create(IDependencyContainer dependency)
        {
            var root = new GameObject("UguiRoot").AddComponent<UguiRoot>();
            dependency?.Inject(root);
            return root;
        }

        protected override void Awake()
        {
            base.Awake();

            myObject.layer = 5;

            canvas = myObject.AddComponent<Canvas>();
            scaler = myObject.AddComponent<CanvasScaler>();
            raycaster = myObject.AddComponent<GraphicRaycaster>();

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }

        public void SetOverlayRender(int sortOrder = 0)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortOrder;
        }

        public void SetCameraRender(Camera camera)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
        }
    }
}