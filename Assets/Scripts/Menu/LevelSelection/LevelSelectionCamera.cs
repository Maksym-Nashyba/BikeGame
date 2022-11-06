using System.Threading.Tasks;
using UnityEngine;

namespace Menu
{
    public class LevelSelectionCamera : MonoBehaviour
    {
        public CameraCheckpoint GetCheckpointFor(int levelIndex) => _checkpoints[levelIndex];
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private CameraCheckpoint[] _checkpoints;

        public void TeleportToLevel(int levelIndex)
        {
            Vector3 checkpointPosition = _checkpoints[levelIndex].Position;
            _cameraTransform.SetPositionAndRotation(checkpointPosition, GetLookRotation(checkpointPosition, _checkpoints[levelIndex].Target));
        }
        
        public Task MoveToLevel(int targetLevelIndex)
        {
            TeleportToLevel(targetLevelIndex);
            return Task.CompletedTask;
        }

        private Quaternion GetLookRotation(Vector3 from, Vector3 to)
        {
            Vector3 direction = to - from;
            return Quaternion.LookRotation(direction);
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