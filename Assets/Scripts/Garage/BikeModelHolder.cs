using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Garage
{
    public class BikeModelHolder : MonoBehaviour
    {
        public BikePositions _currentPosition;
        [SerializeField] private Transform _holderTransform;
        [SerializeField] private Transform _previewPosition;
        [SerializeField] private Transform _paintPosition;

        public Task MoveToPosition(BikePositions target)
        {
            throw new NotImplementedException();
        }
        
        private async Task MoveToPosition(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public enum BikePositions
        {
            Preview,
            Paint
        }
    }
}