using System.Threading.Tasks;
using IGUIDResources;
using Misc;
using UnityEngine;

namespace Menu.Garage.Paint.Containers
{
    public class PaintContainerAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _paintTransform;
        [SerializeField] private Transform _leakTransform;
        [SerializeField] private MeshRenderer _paintRenderer;
        [SerializeField] private MeshRenderer _leakRenderer;
        private AsyncExecutor _asyncExecutor;

        protected void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
        }
        
        protected void OnDestroy()
        {
            _asyncExecutor.Dispose();
        }

        public void ApplySkin(Skin skin)
        {
            _paintRenderer.material = skin.Material;
            _leakRenderer.material = skin.Material;
        }

        public Task PlayFillAnimation()
        {
            return Task.CompletedTask;
        }

        public Task PlayCleanAnimation()
        {
            return Task.CompletedTask;
        }
    }
}