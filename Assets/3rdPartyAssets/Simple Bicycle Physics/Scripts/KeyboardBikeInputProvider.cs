using UnityEngine;

namespace SBPScripts
{
    public class KeyboardBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        public InputValues GetCurrentInput(Transform bikeTransform)
        {
            float steer = Input.GetAxis("Horizontal");
            float acceleration = Input.GetAxis("Vertical");
            bool brakes = Input.GetKey(KeyCode.C);
            return new InputValues(steer, acceleration, brakes);
        }
    }
}