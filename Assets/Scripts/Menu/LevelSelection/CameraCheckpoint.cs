using UnityEngine;

namespace Menu
{
    public class CameraCheckpoint : MonoBehaviour
    {
        public Vector3 Target => _targetTransform.position;
        public Vector3 Position => _positionTransform.position;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _positionTransform;
        
        
    }
}