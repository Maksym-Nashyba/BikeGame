using UnityEngine;

namespace ProgressionStore.Paint
{
    public class ClosedSide : RotatingButtonSide
    {
        public ClosedSide(Transform buttonTransform) : base(buttonTransform)
        {
        }
        
        public override void OnPressed()
        {
        }

        public override float GetTargetRotation()
        {
            throw new System.NotImplementedException();
        }
    }
}