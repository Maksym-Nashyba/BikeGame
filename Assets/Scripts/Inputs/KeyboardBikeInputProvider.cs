using UnityEngine;

namespace Inputs
{
    public class KeyboardBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        private PlayerNewInput _playerNewInput;

        private void Awake()
        {
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
            float steer = _playerNewInput.Player.Move.ReadValue<Vector2>().x;
            float acceleration = _playerNewInput.Player.Move.ReadValue<Vector2>().y;
            bool brakes = _playerNewInput.Player.Brake.IsPressed();
            bool sprint = _playerNewInput.Player.Sprint.IsPressed();
            return new InputValues(steer, acceleration, brakes, sprint);
        }
    }
}