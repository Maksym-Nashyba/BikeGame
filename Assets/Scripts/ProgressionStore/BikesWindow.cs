using UnityEngine;

namespace ProgressionStore
{
    public class BikesWindow : ShopWindow
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _closeButton;

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Hidden")) return;
            _animator.Play("Open");
            _closeButton.SetActive(true);
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Shown")) return;
            _animator.Play("Close");
            _closeButton.SetActive(false);
        }
    }
}