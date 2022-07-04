using UnityEngine;

namespace SBPScripts
{
    public class KeyboardBikeInputProvider : MonoBehaviour, IBikeInputProvider
    {
        public InputValues GetCurrentInput(Transform bikeTransform)
        {
            float steer = Input.GetAxis("Horizontal");
            float acceleration = Input.GetAxis("Vertical");
            return new InputValues(steer, acceleration);
        }
    }
}