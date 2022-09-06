using UnityEngine;

namespace ProgressionStore.Paint
{
    class BuySide : RotatingButtonSide
    {
        public BuySide(Transform buttonTransform) : base(buttonTransform)
        {
        }
        
        public override void OnPressed()
        {
            
        }

        public override float GetTargetRotation()
        {
            return -90f;
        }
    }
}