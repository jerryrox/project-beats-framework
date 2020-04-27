using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Acceleration implementation using cursor's position relative within the screen.
    /// </summary>
    public class CursorAccelerator : IAccelerator {

        private Vector2 acceleration = new Vector2();


        public Vector2 Acceleration => acceleration;


        public void Update()
        {
            float halfWidth = Screen.width * 0.5f;
            float halfHeight = Screen.height * 0.5f;
            Vector3 pos = Input.mousePosition;
            acceleration.x = (pos.x - halfWidth) / halfWidth;
            acceleration.y = (pos.y - halfHeight) / halfHeight;
        }
    }
}