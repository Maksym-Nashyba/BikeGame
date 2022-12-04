using System;
using System.Threading.Tasks;
using Garage;
using Misc;
using UnityEngine;

namespace ProgressionStore
{
    public class GarageCamera : MonoBehaviour
    {
        public event Action ArrivedAtCheckpoint;
        public event Action DepartedFromCheckpoint;
        public bool IsMoving { get; private set; }
        public bool IsAtRestPoint => _currentCheckpoint == _restCheckpoint;
        [SerializeField] private float _transitionDurationSeconds;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private CameraCheckpoint _restCheckpoint;
        [SerializeField] private CameraCheckpoint[] _checkpoints;
        [SerializeField] private CameraCheckpointClickTarget[] _clickTargets;
        [SerializeField] private GarageUI _garageUI;
        private AsyncExecutor _asyncExecutor;
        private CameraCheckpoint _currentCheckpoint;
        private bool CanMoveFromRest => IsAtRestPoint && !IsMoving;

        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
            _garageUI.BackButtonClicked += OnBackButton;
            foreach (CameraCheckpointClickTarget target in _clickTargets)
            {
                target.Clicked += OnTargetClicked;
            }
        }

        private void Start()
        {
            _currentCheckpoint = _restCheckpoint;
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
            _garageUI.BackButtonClicked += OnBackButton;
            foreach (CameraCheckpointClickTarget target in _clickTargets)
            {
                target.Clicked -= OnTargetClicked;
            }
        }
        
        private async void OnTargetClicked(CameraCheckpoint target)
        {
            if(!CanMoveFromRest || _currentCheckpoint == target) return;

            await MoveToCheckpoint(target, _transitionDurationSeconds);
        }

        private async void OnBackButton()
        {
            if(IsAtRestPoint || IsMoving) return;

            await MoveToCheckpoint(_restCheckpoint, _transitionDurationSeconds);
        } 

        private async Task MoveToCheckpoint(CameraCheckpoint targetCheckpoint, float duration)
        {
            IsMoving = true;
            _currentCheckpoint.SendCameraDeparted();
            targetCheckpoint.SendCameraApproaching();
            DepartedFromCheckpoint?.Invoke();
            
            await LerpCameraToCheckpoint(targetCheckpoint, _transitionDurationSeconds);
            
            targetCheckpoint.SendCameraArrived();
            _currentCheckpoint = targetCheckpoint;
            ArrivedAtCheckpoint?.Invoke();
            IsMoving = false;
        }
        
        private Task LerpCameraToCheckpoint(CameraCheckpoint targetCheckpoint, float duration)
        {
            return _asyncExecutor.EachFrame(duration, t =>
            {
                Transformation transformation = _currentCheckpoint.Lerp(targetCheckpoint, t);
                _cameraTransform.SetPositionAndRotation(transformation.Position, transformation.Rotation);
            }, EaseFunctions.InOutQuad);
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