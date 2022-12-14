using System.Threading.Tasks;
using IGUIDResources;
using UnityEngine;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private MeshRenderer _paintRenderer;
        [SerializeField] private MeshRenderer _leakRenderer;

        public void ApplySkin(Skin skin)
        {
            _paintRenderer.material = skin.Material;
            _leakRenderer.material = skin.Material;
        }

        public Task PlayFillAnimation()
        {
            _animator.Play("Fill");
            return Task.CompletedTask;
        }

        public Task PlayCleanAnimation()
        {
            _animator.Play("Clean");
            return Task.CompletedTask;
        }
    }
}