using UnityEngine;

namespace ProgressionStore
{
    public class BikesGarageShop : GarageShop
    {
        [SerializeField] private Animator _animator;

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenShopWindow");
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownShopWindow")) return;
            _animator.Play("CloseShopWindow");
        }
    }
}