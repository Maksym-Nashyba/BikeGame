using UnityEngine;

namespace ProgressionStore
{
    public class PaintGarageShop : GarageShop
    {
        [SerializeField] private Animator _animator;

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenPaintWindow");
            GarageUI.SetGeneralUIActive(false);
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownPaintWindow")) return;
            _animator.Play("ClosePaintWindow");
            GarageUI.SetGeneralUIActive(true);
        }
    }
}