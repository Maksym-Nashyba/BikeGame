using UnityEngine;

namespace ProgressionStore
{
    public class PaintGarageShop : GarageShop
    {
        [SerializeField] private Animator _animator;
        private GarageUI _garageUI;

        protected override void Awake()
        {
            base.Awake();
            _garageUI = FindObjectOfType<GarageUI>();
            _garageUI.SetBikeSelectionEnabled(false);
        }

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenPaintWindow");
            _garageUI.SetBikeSelectionEnabled(true);
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownPaintWindow")) return;
            _animator.Play("ClosePaintWindow");
            _garageUI.SetBikeSelectionEnabled(false);
        }
    }
}