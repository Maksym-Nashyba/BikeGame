using System;
using Joystick_Pack.Scripts.Base;
using Misc;
using UnityEditor;

using UnityEngine;

namespace SBPScripts
{
    public class JoystickBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Joystick _joystick;

        public InputValues GetCurrentInput(Transform bikeTransform)
        {
            Vector3 bikeforward = bikeTransform.forward;
            Vector2 flatBikeDirection = new Vector2(bikeforward.x, bikeforward.z);
            Vector3 cameraDirection = GetCameraDirection();
            Vector2 joystickValue = GetJoystickValue();
            
            float joystickAngle = Vector2.SignedAngle(Vector2.up, joystickValue);
            Vector2 flatCameraDirection = new Vector2(cameraDirection.x, cameraDirection.z);
            Vector2 targetDirection = flatCameraDirection.RotatedBy(joystickAngle);
            float steer = Vector2.SignedAngle(flatBikeDirection, targetDirection)/180f;
            float acceleration = joystickValue.magnitude;
            return new InputValues(-steer, acceleration);
        }

        private Vector3 GetCameraDirection()
        {
            return _cameraTransform.forward;
        }

        private Vector2 GetJoystickValue()
        {
            return _joystick.Direction;
        }
    }
}