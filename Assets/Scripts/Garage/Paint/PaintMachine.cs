using Misc;
using UnityEngine;

namespace Garage.Paint
{
    public class PaintMachine : MonoBehaviour
    {
        [SerializeField] private BikeModelDisplay _modelDisplay;
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;

        private void Awake()
        {
            _cameraCheckpoint.CameraApproaching += OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted += OnCameraDeparted;
        }
        
        private void OnDestroy()
        {
            _cameraCheckpoint.CameraApproaching -= OnCameraApproaching;
            _cameraCheckpoint.CameraDeparted -= OnCameraDeparted;
        }

        private void OnCameraApproaching()
        {
            _modelDisplay.Holder.MoveToPosition(BikeModelHolder.BikePositions.Paint);
        }
        
        private void OnCameraDeparted()
        {
            _modelDisplay.Holder.MoveToPosition(BikeModelHolder.BikePositions.Preview);
        }
    }
}