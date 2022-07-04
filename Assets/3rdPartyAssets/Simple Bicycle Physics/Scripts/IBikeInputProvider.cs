using UnityEngine;

namespace SBPScripts
{
    public interface IBikeInputProvider
    {
        public InputValues GetCurrentInput(Transform bikeTransform);
    }
}