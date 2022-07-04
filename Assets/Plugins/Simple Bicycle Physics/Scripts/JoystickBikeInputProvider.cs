using Joystick_Pack.Scripts.Base;
using UnityEngine;

namespace SBPScripts
{
    public class JoystickBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Joystick _joystick;

        public InputValues GetCurrentInput(Transform bikeTransform)
        {
            Vector3 bikeDirection = bikeTransform.forward;
            Vector3 cameraDirection = GetCameraDirection();
            Vector3 joystickValue = GetJoystickValue();
            float steer = joystickValue.x;
            float acceleration = joystickValue.y;

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