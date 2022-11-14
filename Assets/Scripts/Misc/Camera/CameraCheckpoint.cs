using System;
using UnityEngine;

namespace Misc
{
    public class CameraCheckpoint : MonoBehaviour
    {
        public event Action CameraArrived;
        public event Action CameraDeparted;
        public Vector3 Target => _targetTransform.position;
        public Vector3 Position => _positionTransform.position;
        
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _positionTransform;
    }
}