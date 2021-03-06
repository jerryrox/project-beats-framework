using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PBFramework.Dependencies;

namespace PBFramework.Graphics
{
    public class UguiRoot : UguiObject, IRoot {

        private CanvasScaler scaler;


        public Vector2 Resolution => this.Size;

        public Vector2 BaseResolution
        {
            get => scaler.referenceResolution;
            set => scaler.referenceResolution = value;
        }

        public Camera Cam => Canvas.worldCamera;

        public GraphicRaycaster Raycaster { get; protected set; }

        public EventSystem EventSystem { get; protected set; }


        /// <summary>
        /// Creates a new instance of the UguiRoot and returns it.
        /// Optionally provide a dependency container to pass down dependencies.
        /// </summary>
        public static UguiRoot Create(IDependencyContainer dependency)
        {
            var root = new GameObject("UguiRoot").AddComponent<UguiRoot>();
            if (dependency != null)
            {
                if(!dependency.Contains<IRoot>())
                    dependency.CacheAs<IRoot>(root);
                dependency.Inject(root);
            }
            return root;
        }

        protected override void Awake()
        {
            base.Awake();

            myObject.layer = 5;

            Canvas = myObject.AddComponent<Canvas>();
            scaler = myObject.AddComponent<CanvasScaler>();
            Raycaster = myObject.AddComponent<GraphicRaycaster>();

            Canvas.planeDistance = 1f;
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            var es = GameObject.FindObjectOfType<EventSystem>();
            if (es == null)
            {
                es = new GameObject("EventSystem").AddComponent<EventSystem>();

                var standaloneModule = es.GetComponent<StandaloneInputModule>();
                if (standaloneModule == null)
                {
                    standaloneModule = es.gameObject.AddComponent<StandaloneInputModule>();
                    es.sendNavigationEvents = false;
                    es.pixelDragThreshold = 10;
                    standaloneModule.inputActionsPerSecond = 10;
                    standaloneModule.repeatDelay = 0.5f;
                }
            }
            this.EventSystem = es;
        }

        public void SetOverlayRender(int sortOrder = 0)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Canvas.sortingOrder = sortOrder;
        }

        public void SetCameraRender(Camera camera)
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.worldCamera = camera;
        }
    }
}