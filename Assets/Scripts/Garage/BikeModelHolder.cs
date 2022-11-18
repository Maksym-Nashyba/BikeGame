using System.Threading.Tasks;
using UnityEngine;

namespace Garage
{
    public class BikeModelHolder : MonoBehaviour
    {
        public Transform HolderTransform => _holderTransform;
        public BikePositions CurrentPosition => _currentPosition;
        [SerializeField] private BikePositions _currentPosition;
        [SerializeField] private float _transitionDuration;
        [SerializeField] private Transform _holderTransform;
        [SerializeField] private Transform _previewPosition;
        [SerializeField] private Transform _paintPosition;

        private async void Start()
        {
            await MoveToPosition(CurrentPosition);
        }

        public Task MoveToPosition(BikePositions target)
        {
            Transform targetTransform = target == BikePositions.Preview ? _previewPosition : _paintPosition;
            return MoveToPosition(targetTransform.position, _transitionDuration);
        }
        
        private async Task MoveToPosition(Vector3 targetPosition, float duration)
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

        public enum BikePositions
        {
            Preview,
            Paint
        }
    }
}