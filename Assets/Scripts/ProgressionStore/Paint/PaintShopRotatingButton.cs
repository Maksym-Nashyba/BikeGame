using UnityEngine;

namespace ProgressionStore.Paint
{
    public class PaintShopRotatingButton : MonoBehaviour
    {
        public bool IsSpinning { get; private set; }

        private Transform _transform;
        private ClosedSide _closedSide;
        private BuySide _buySide;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _closedSide = new ClosedSide(_transform);
            _buySide = new BuySide(_transform);
        }

        public async void OnPressed()
        {
            if(IsSpinning) return;

            _buySide.RotateTo();
        }
    }
}