using UnityEngine;

namespace ProgressionStore
{
    public class BikesWindow : ShopWindow
    {
        [SerializeField] private Animator _animator;

        public override void Open()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Hidden")) return;
            _animator.Play("Open");
        }

        public override void Close()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Shown")) return;
            _animator.Play("Close");
        }
    }
}