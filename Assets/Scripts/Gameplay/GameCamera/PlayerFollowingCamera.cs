using System.Collections.Generic;
using Misc;
using Pausing;
using UnityEngine;

namespace Gameplay.GameCamera
{
    public class PlayerFollowingCamera : MonoBehaviour,IPausable
    {
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private Vector3 _direction;
        private Rigidbody _playerRigidbody;
        private Transform _playerTransform;
        private Transform _cameraTransform;
        private bool _isPaused;
        private float _lastCameraElevation;
        private Queue<Vector3> _lookaheadOffsetHistory;
        private int _checksLayerMask;

        private void Awake()
        {
            ServiceLocator.Player.Respawned += ResolveDependencies;
            _cameraTransform = GetComponent<Transform>();
            _direction = _direction.normalized;
            _lookaheadOffsetHistory = new Queue<Vector3>(10);
            _checksLayerMask = LayerMask.GetMask("Landscape", "Props", "Player", "Default");
        }

        private void ResolveDependencies()
        {
            _playerTransform = ServiceLocator.Player.ActivePlayerClone.GetComponent<Transform>();
            _playerRigidbody = ServiceLocator.Player.ActivePlayerClone.GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            if (_isPaused) return;
            Vector3 nextCameraPosition = GetNextCameraPosition();
            MoveCameraToPosition(nextCameraPosition);
        }

        
        private Vector3 GetNextCameraPosition()
        {
            if (!ServiceLocator.Player.IsAlive) return _cameraTransform.position;
            if (_playerTransform is null) return Vector3.zero;
            Vector3 nextCameraPosition = _playerTransform.position + OffsetFromPlayerPosition();
            nextCameraPosition = ApplyLookahead(nextCameraPosition);
            nextCameraPosition = MoveToAvoidCollisions(nextCameraPosition);
            return nextCameraPosition;
        }

        private Vector3 MoveToAvoidCollisions(Vector3 rawCameraPosition)
        {
            Vector3 raycastOrigin = _cameraTransform.position;
            Vector3 raycastTarget = _playerTransform.position;
            Vector3 originToTarget = raycastTarget - raycastOrigin;
            
            Ray ray = new Ray(raycastOrigin, originToTarget);
            bool directlyObstructed = Physics.Raycast(ray, out RaycastHit hit, originToTarget.magnitude - 1f, _checksLayerMask);
            _lastCameraElevation += directlyObstructed ? Time.deltaTime * 4f : -Time.deltaTime;

            if (directlyObstructed && hit.transform.TryGetComponent(out CameraObstruction transparencyToggle))
            {
                transparencyToggle.MakeTransparentForSomeTime();
            }

            Vector3 velocity = _playerRigidbody.velocity.WithZeroY();
            if (velocity.magnitude > 2f)
            {
                raycastOrigin += velocity / 3f;
                raycastTarget += velocity / 3f;
                originToTarget = raycastTarget - raycastOrigin;
                
                Ray predictionRay = new Ray(raycastOrigin, originToTarget);
                bool forwardObstructed = Physics.Raycast(predictionRay,originToTarget.magnitude - 1f, _checksLayerMask);
                _lastCameraElevation += forwardObstructed ? Time.deltaTime * 4f : -Time.deltaTime;
            }
            
            _lastCameraElevation = Mathf.Clamp(_lastCameraElevation, 0, 5f);
            return rawCameraPosition + Vector3.up * _lastCameraElevation;
        }

        private Vector3 ApplyLookahead(Vector3 rawCameraPosition)
        {
            Vector3 velocity = _playerRigidbody.velocity;
            float speed = velocity.magnitude / 20f;
            velocity = velocity.WithZeroY().normalized;

            Vector3 cameraRight = _cameraTransform.right.WithZeroY();
            float lookaheadToCameraAngle = Vector3.Angle(cameraRight, velocity) / 180f;
            Vector3 offset = velocity * (7.5f * speed * (1f- Mathf.Sin(lookaheadToCameraAngle * Mathf.PI) / 1.3f));

            _lookaheadOffsetHistory.Enqueue(offset);
            Vector3 historyAverageOffset = GetAverage(_lookaheadOffsetHistory);
            offset += historyAverageOffset;
            offset /= 2f;
            
            return rawCameraPosition + offset;
        }

        private Vector3 OffsetFromPlayerPosition()
        {
            float distance = _minDistance + (_maxDistance - _minDistance) * _playerRigidbody.velocity.magnitude / 20f;
            return distance * _direction;   
        }

        private void MoveCameraToPosition(Vector3 nextCameraPosition)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, nextCameraPosition, 500f * Time.deltaTime);
        }

        private Vector3 GetAverage(Queue<Vector3> queue)
        {
            Vector3 result = Vector3.zero;
            foreach (Vector3 vector in queue)
            {
                result += vector;
            }

            return result / queue.Count;
        }
        
        private void OnDisable()
        {
            ServiceLocator.Player.Respawned -= ResolveDependencies;
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
