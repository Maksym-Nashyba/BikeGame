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
            Vector3 bikeForward = bikeTransform.forward;
            Vector2 flatBikeDirection = new Vector2(bikeForward.x, bikeForward.z);
            Vector3 cameraDirection = GetCameraDirection();
            Vector2 joystickValue = GetJoystickValue();
            
            float joystickAngle = Vector2.SignedAngle(Vector2.up, joystickValue);
            Vector2 flatCameraDirection = new Vector2(cameraDirection.x, cameraDirection.z);
            Vector2 targetDirection = flatCameraDirection.RotatedBy(joystickAngle);
            
            float steer = Vector2.SignedAngle(flatBikeDirection, targetDirection)/180f;
            float acceleration = Mathf.Pow(joystickValue.magnitude,4f) * (1f-Mathf.Abs(steer));
            float sign = -1 * Mathf.Sign(steer);
            steer = Mathf.Max(1f+Mathf.Log10(Mathf.Abs(steer)), 0f) * sign;
            if (acceleration < 0.01f) steer = 0;
            Debug.DrawRay(bikeTransform.position, bikeForward, Color.magenta, 20f);
            return new InputValues(steer, acceleration);
        }

        private Vector3 GetCameraDirection()
        {
            return _cameraTransform.forward;
        }

        private Vector2 GetJoystickValue()
        {
            return _joystick.Direction;
        }
        
        public static float Remap (float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
    }
}