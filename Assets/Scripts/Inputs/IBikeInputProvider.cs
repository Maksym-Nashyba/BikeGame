using UnityEngine;

namespace Inputs
{
    public interface IBikeInputProvider
    {
        public InputValues GetCurrentInput(Transform bikeTransform);
    }
}