using Misc;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Inputs
{
    public class JoystickBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private GameObject _joystickObject;
        private IJoystick _joystick;

        private PlayerNewInput _playerNewInput;

        private void Awake()
        {
            _joystick = _joystickObject.GetComponent<IJoystick>();
            _playerNewInput = new PlayerNewInput();
        }
        
        private void OnEnable()
        {
            _playerNewInput.Enable();
        }

        private void OnDisable()
        {
            _playerNewInput.Disable();
        }
        
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

            steer = Mathf.Sqrt(Mathf.Abs(steer)) * sign;
            if (acceleration < 0.01f) steer = 0;
            
            bool brakes = GetBrakesHit();
            bool sprint = GetSprintHit();

            Debug.DrawRay(bikeTransform.position + Vector3.up*0.5f, targetDirection, Color.red);
            Vector2 flatBikeForward = new Vector2(bikeForward.x, bikeForward.z).RotatedBy(-90f * steer);
            Vector3 bikeSteerDirection = new Vector3(flatBikeForward.x, 0f, flatBikeForward.y);
            Debug.DrawRay(bikeTransform.position + Vector3.up*0.5f, bikeSteerDirection, Color.cyan);
            
            
            
            return new InputValues(steer, acceleration, brakes, sprint);  
        }

        private Vector3 GetCameraDirection()
        {
            return _cameraTransform.forward;
        }

        private Vector2 GetJoystickValue()
        {
            return _joystick.GetDirection();
        }

        private bool GetBrakesHit()
        {
            return _playerNewInput.Player.Brake.IsPressed();
        }
        
        private bool GetSprintHit()
        {
            return _playerNewInput.Player.Sprint.IsPressed();
        }
    }
}