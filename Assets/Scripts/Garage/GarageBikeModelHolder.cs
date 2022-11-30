using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Garage
{
    public class GarageBikeModelHolder : BikeModelHolder
    {
        public BikePositions CurrentPosition => _currentPosition;
        [SerializeField] private BikePositions _currentPosition;
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
        
        public enum BikePositions
        {
            Preview,
            Paint
        }
    }
}