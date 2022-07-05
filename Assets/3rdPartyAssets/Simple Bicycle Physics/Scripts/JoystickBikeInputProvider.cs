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
            float acceleration = joystickValue.magnitude;
            float sign = -1 * Mathf.Sign(steer);
            steer = Mathf.Min(-Mathf.Log10(Mathf.Abs(steer)), 1f) * sign;
            //steer = Mathf.Pow(steer, 0.5f) * sign;
            Debug.DrawRay(bikeTransform.position, new Vector3(targetDirection.x, 0, targetDirection.y) * 3, Color.red);

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
    }
}