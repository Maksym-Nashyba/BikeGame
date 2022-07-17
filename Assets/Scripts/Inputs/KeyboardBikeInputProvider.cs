using UnityEngine;

namespace Inputs
{
    public class KeyboardBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        public InputValues GetCurrentInput(Transform bikeTransform)
        {
            float steer = Input.GetAxis("Horizontal");
            float acceleration = Input.GetAxis("Vertical");
            bool brakes = Input.GetKey(KeyCode.C);
            bool sprint = Input.GetKey(KeyCode.LeftShift);
            return new InputValues(steer, acceleration, brakes, sprint);
        }
    }
}