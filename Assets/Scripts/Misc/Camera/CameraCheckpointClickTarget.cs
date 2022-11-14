using System;
using UnityEngine;

namespace Misc
{
    public class CameraCheckpointClickTarget : ClickTarget<CameraCheckpoint>
    {
        public override event Action<CameraCheckpoint> Clicked;
        
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;

        protected override void OnClicked()
        {
            Clicked?.Invoke(_cameraCheckpoint);
        }
    }
}