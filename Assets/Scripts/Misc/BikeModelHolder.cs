using System.Threading.Tasks;
using UnityEngine;

namespace Misc
{
    public abstract class BikeModelHolder : MonoBehaviour
    {
        public Transform HolderTransform => _holderTransform;
        [SerializeField] protected float _transitionDuration;
        [SerializeField] protected Transform _holderTransform;

        protected async Task MoveToPosition(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = _holderTransform.position;
            float timePassed = 0;
            while (timePassed < duration)
            {
                _holderTransform.position = Vector3.Lerp(startPosition, targetPosition,timePassed/duration);
                await Task.Yield();
                timePassed += Time.deltaTime;
            }
        }
    }
}