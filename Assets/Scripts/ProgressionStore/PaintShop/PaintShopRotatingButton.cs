using UnityEngine;

namespace ProgressionStore.PaintShop
{
    public class PaintShopRotatingButton : MonoBehaviour
    {
        public RotatingButtonSides CurrentSide { get; private set; }

        
        
        private float SideToRotation(RotatingButtonSides side)
        {
            return (int)side * 90f;
        }
        
        public enum RotatingButtonSides
        {
            Closed,
            Buy,
            Confirm,
            Select
        }
    }
}