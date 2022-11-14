using System;
using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace ProgressionStore
{
    public class GarageCamera : MonoBehaviour
    {
        public bool IsMoving { get; private set; }
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private CameraCheckpoint _restCheckpoint;
        [SerializeField] private CameraCheckpoint[] _checkpoints;
        [SerializeField] private CameraCheckpointClickTarget[] _clickTargets;
        private CameraCheckpoint _currentCheckpoint;
        private bool IsAtRestPoint => _currentCheckpoint == _restCheckpoint;
        private bool CanMoveFromRest => IsAtRestPoint && !IsMoving;

        private void Awake()
        {
            foreach (CameraCheckpointClickTarget target in _clickTargets)
            {
                target.Clicked += OnTargetClicked;
            }
        }

        private void Start()
        {
            _currentCheckpoint = _restCheckpoint;
        }

        private async void OnTargetClicked(CameraCheckpoint target)
        {
            Debug.Log($"Clicked {target.name}");
            if(!CanMoveFromRest || _currentCheckpoint == target) return;

            IsMoving = true;
            await MoveToCheckpoint(target, 2f);
            IsMoving = false;
        }

        private Task MoveToCheckpoint(CameraCheckpoint targetCheckpoint, float duration)
        {
            
            
            return Task.CompletedTask;
        }
        
        private void OnDestroy()
        {
            foreach (CameraCheckpointClickTarget target in _clickTargets)
            {
                target.Clicked -= OnTargetClicked;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(_checkpoints is null || _checkpoints.Length == 0) return;
            
            foreach (CameraCheckpoint checkpoint in _checkpoints)
            {
                if(checkpoint == null) continue;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_restCheckpoint.Position, checkpoint.Position);
                DrawCheckPoint(checkpoint);
            }

            if (_restCheckpoint != null) DrawCheckPoint(_restCheckpoint);
        }

        private void DrawCheckPoint(CameraCheckpoint checkpoint)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(checkpoint.Position, checkpoint.Target);
        }
#endif
    }
}