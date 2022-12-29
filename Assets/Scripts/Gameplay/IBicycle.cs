using System;
using UnityEngine;

namespace Gameplay
{
    public interface IBicycle
    {
        public event Action Landed;
        
        public float GetCurrentSpeed();

        public float GetAcceleration();

        public float GetTorqueY();

        public void SetInteractable(bool enabled);

        public bool IsAirborne();

        public float GetAirtimeSeconds();
        
        public Vector3 GetCurrentVelocity();
    }
}