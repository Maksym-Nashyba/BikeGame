using UnityEngine;

namespace GameCamera
{
    public class TargetFollowingCamera : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _cameraSpeed;
        private Transform _cameraTransform;
        private Vector3 _offset;
    
        private void Awake()
        {
            _cameraTransform = GetComponent<Transform>();
        }

        private void Start()
        {
            _offset = _cameraTransform.position - _playerTransform.position;
        }

        private void LateUpdate()
        {
            float playerSpeed = _playerRigidbody.velocity.magnitude;
            Vector3 nextCameraPosition = _playerTransform.position + _offset;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, nextCameraPosition, _cameraSpeed * Time.deltaTime);
        }
    }
}
