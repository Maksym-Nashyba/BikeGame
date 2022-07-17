using Pausing;
using UnityEngine;

namespace GameCamera
{
    public class TargetFollowingCamera : MonoBehaviour,IPausable
    {
        [SerializeField] private Rigidbody _playerRigidbody; 
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        private Transform _playerTransform;
        private Transform _cameraTransform;
        private Vector3 _direction;
        private bool _isPaused;
    
        private void Awake()
        {
            _cameraTransform = GetComponent<Transform>();
            _playerTransform = _playerRigidbody.GetComponent<Transform>();
        }

        private void Start()
        {
            _direction = CalculateDirection();
        }

        private void LateUpdate()
        {
            if (_isPaused) return;
            Vector3 nextCameraPosition = GetNextCameraPosition();
            MoveCameraToPosition(nextCameraPosition);
        }
        
        private Vector3 CalculateDirection()
        {
            Vector3 direction = _cameraTransform.position - _playerTransform.position;
            direction = direction.normalized;
            return direction;
        }
        
        private Vector3 GetNextCameraPosition()
        {
            float distance = _minDistance + (_maxDistance - _minDistance) * _playerRigidbody.velocity.magnitude / 20f;
            Vector3 nextCameraPosition = _playerTransform.position + (distance * _direction);
            return nextCameraPosition;
        }

        private void MoveCameraToPosition(Vector3 nextCameraPosition)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, nextCameraPosition, 500f * Time.deltaTime);
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Continue()
        {
            _isPaused = false;
        }
    }
}
