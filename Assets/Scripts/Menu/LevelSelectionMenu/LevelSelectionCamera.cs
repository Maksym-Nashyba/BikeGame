using System;
using System.Threading.Tasks;
using Effects.TransitionCover;
using LevelLoading;
using Misc;
using UnityEngine;

namespace Menu
{
    public class LevelSelectionCamera : MonoBehaviour
    {
        public LevelSelectionCheckpoint GetCheckpointFor(int levelIndex) => _checkpoints[levelIndex];
        [Range(0f, 3f)][SerializeField] private float _transitionDurationSeconds;
        [Range(0f, 10f)][SerializeField] private float _launchZoomInDurationSeconds;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private LevelSelectionCheckpoint[] _checkpoints;
        [SerializeField] private LevelSelection _levelSelection;
        [SerializeField] private SceneTransitionCover _cloudSceneTransitionCover;
        private AsyncExecutor _asyncExecutor;
        private LevelSelectionCheckpoint CurrentCheckpoint => _checkpoints[_currentCheckpointIndex];
        private int _currentCheckpointIndex;

        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
            _levelSelection.LaunchingLevel += PlayLaunchEffects;
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
        }

        public void TeleportToLevel(int levelIndex)
        {
            Vector3 checkpointPosition = _checkpoints[levelIndex].Position;
            _cameraTransform.SetPositionAndRotation(checkpointPosition, GetLookRotation(checkpointPosition, _checkpoints[levelIndex].Target));
            _currentCheckpointIndex = levelIndex;
        }
        
        public async Task MoveToLevel(int targetLevelIndex)
        {
            while (targetLevelIndex != _currentCheckpointIndex)
            {
                int stepDirection = Convert.ToInt32(_currentCheckpointIndex < targetLevelIndex) * 2 -1;
                await MoveByOneCheckpoint(stepDirection);
                _currentCheckpointIndex += stepDirection;
            }
        }

        public async Task PlayLaunchEffects()
        {
            _levelSelection.LaunchingLevel -= PlayLaunchEffects;
            ZoomInOnCheckpoint(CurrentCheckpoint);
            await _cloudSceneTransitionCover.TransitionToState(SceneTransitionCover.State.Covered);
        }
        
        private Task MoveByOneCheckpoint(int direction)
        {
            CameraCheckpoint startCheckpoint = CurrentCheckpoint;
            CameraCheckpoint targetCheckpoint = _checkpoints[_currentCheckpointIndex+direction];

            return _asyncExecutor.EachFrame(_transitionDurationSeconds, t =>
            {
                Vector3 nextPosition = Vector3.LerpUnclamped(startCheckpoint.Position, targetCheckpoint.Position, t);
                Vector3 nextCameraTarget = Vector3.LerpUnclamped(startCheckpoint.Target, targetCheckpoint.Target, t);
                Quaternion nextQuaternion = GetLookRotation(_cameraTransform.position, nextCameraTarget);
                _cameraTransform.SetPositionAndRotation(nextPosition, nextQuaternion);
            }, EaseFunctions.InOutQuad);
        }

        private Quaternion GetLookRotation(Vector3 from, Vector3 to)
        {
            Vector3 direction = to - from;
            return Quaternion.LookRotation(direction);
        }

        private Task ZoomInOnCheckpoint(LevelSelectionCheckpoint checkpoint)
        {
            float distanceToCheckpoint = (checkpoint.Position - checkpoint.Target).magnitude;
            return _asyncExecutor.EachFrame(_launchZoomInDurationSeconds, t =>
            {
                _cameraTransform.position =
                    checkpoint.Position + _cameraTransform.forward * (distanceToCheckpoint / 3f * t);
            }, EaseFunctions.InOutBack);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < _checkpoints.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(_checkpoints[i].Position, _checkpoints[i].Target);
                Gizmos.color = Color.magenta;
                CameraCheckpoint checkpoint = _checkpoints[i];
                Gizmos.DrawSphere(checkpoint.Position, 0.1f);
                if(i == _checkpoints.Length-1)continue;
                Gizmos.DrawLine(_checkpoints[i].Position, _checkpoints[i+1].Position);
            }
        }
        #endif
    }
}