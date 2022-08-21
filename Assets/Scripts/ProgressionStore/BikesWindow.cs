using UnityEngine;

namespace ProgressionStore
{
    public class BikesWindow : ShopWindow
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _closeButton;

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("HiddenCamera")) return;
            _animator.Play("OpenShopWindow");
            _closeButton.SetActive(true);
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ShownShopWindow")) return;
            _animator.Play("CloseShopWindow");
            _closeButton.SetActive(false);
        }
    }
}