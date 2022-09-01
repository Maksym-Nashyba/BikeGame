using UnityEngine;

namespace ProgressionStore.Paint
{
    public abstract class RotatingButtonSide
    {
        protected Transform ButtonTransform;
        
        public RotatingButtonSide(Transform buttonTransform)
        {
            ButtonTransform = buttonTransform;
        }

        public void RotateTo()
        {
        }

        public abstract void OnPressed();

        public abstract float GetTargetRotation();
    }
}