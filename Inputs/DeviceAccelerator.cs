using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Inputs
{
    /// <summary>
    /// Acceleration implementation using the device's actual accelerometer.
    /// </summary>
    public class DeviceAccelerator : IAccelerator
    {

        private Vector2 acceleration = new Vector2();


        public Vector2 Acceleration => acceleration;


        public void Update()
        {
            Vector3 pos = Input.acceleration;
            acceleration.x *= 3.5f;
            acceleration.y = (pos.y + 0.30f) * 3.5f;
        }
    }
}