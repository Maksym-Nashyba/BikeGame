using System.Threading.Tasks;
using UnityEngine;

namespace Misc
{
    public abstract class BikeModelHolder : MonoBehaviour
    {
        public Transform HolderTransform => _holderTransform;
        [SerializeField] protected Transform _holderTransform;
        private AsyncExecutor _asyncExecutor;

        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
        }

        protected Task MoveToPosition(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = _holderTransform.position;
            return _asyncExecutor.EachFrame(duration, t =>
            {
                _holderTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }, EaseFunctions.InOutQuad);
        }
    }
}