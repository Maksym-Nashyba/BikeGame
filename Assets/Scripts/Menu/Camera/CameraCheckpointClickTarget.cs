using System;
using UnityEngine;

namespace Misc
{
    public class CameraCheckpointClickTarget : ClickTarget<CameraCheckpoint>
    {
        public override event Action<CameraCheckpoint> Clicked;
        
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;

        protected override void Awake()
        {
            base.Awake();
            _cameraCheckpoint.CameraArrived += Disable;
            _cameraCheckpoint.CameraDeparted += Enable;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _cameraCheckpoint.CameraArrived -= Disable;
            _cameraCheckpoint.CameraDeparted -= Enable;
        }

        protected override void OnClicked()
        {
            Clicked?.Invoke(_cameraCheckpoint);
        }

        private void Enable()
        {
            enabled = true;
        }
        
        private void Disable()
        {
            enabled = false;
        }
    }
}