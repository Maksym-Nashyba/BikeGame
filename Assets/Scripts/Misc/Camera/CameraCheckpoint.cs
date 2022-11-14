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

        public Transformation Lerp(CameraCheckpoint target, float t)
        {
            Vector3 nextPosition = Vector3.Lerp(Position, target.Position, t);
            Quaternion startDirection = GetLookRotation(Position, Target);
            Quaternion endDirection = GetLookRotation(target.Position, target.Target);
            return new Transformation
            {
                Position = nextPosition,
                Rotation = Quaternion.Slerp(startDirection, endDirection, t)
            };
        }
        
        private Quaternion GetLookRotation(Vector3 from, Vector3 to)
        {
            Vector3 direction = to - from;
            return Quaternion.LookRotation(direction);
        }

        public void SendCameraArrived()
        {
            CameraArrived?.Invoke();
        }

        public void SendCameraDeparted()
        {
            CameraDeparted?.Invoke();
        }
    }
}