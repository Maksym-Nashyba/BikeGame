using Misc;
using Pausing;
using UnityEngine;

namespace GameCamera
{
    public class TargetFollowingCamera : MonoBehaviour,IPausable
    {
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        private Rigidbody _playerRigidbody;
        private Transform _playerTransform;
        private Transform _cameraTransform;
        private Vector3 _direction;
        private bool _isPaused;
    
        private void Awake()
        {
            ServiceLocator.PlayerSpawner.Respawned += ResolveDependencies;
            _cameraTransform = GetComponent<Transform>();
        }

        private void ResolveDependencies(GameObject player)
        {
            _playerTransform = player.transform;
            _playerRigidbody = player.GetComponent<Rigidbody>();
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

        private void OnDisable()
        {
            ServiceLocator.PlayerSpawner.Respawned -= ResolveDependencies;
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
