using Misc;
using UnityEngine;

namespace Menu
{
    public class LevelSelectionCameraCheckpoint : CameraCheckpoint
    {
        public float FogHeiht => _fogHeight;
        [SerializeField] private float _fogHeight;
    }
}