using System;
using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace LevelLoading
{
    public class CameraCloudCover : SceneTransitionCover
    {
        [SerializeField] private Material _cloudOverlayMaterial;
        
        private readonly int _transparencyShaderProperty = Shader.PropertyToID("_Transparency");
        private AsyncExecutor _asyncExecutor;

        private void Awake()
        {
            TranstionToStateInstantly(StartState);
            _asyncExecutor = new AsyncExecutor();
        }

        private void Start()
        {
            if (FireOnStart)
            {
                State opposite = StartState == State.Clean ? State.Covered : State.Clean;
                TransitionToState(opposite);
            }
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
        }

        public void TranstionToStateInstantly(State targetState)
        {
            if (CurrentState == targetState) throw new InvalidOperationException($"State is already {targetState}");

            float t = targetState == State.Clean ? 0f : 1f;
            SetOverlayTransparency(t);
            
            SetState(targetState);
        }

        public async Task TransitionToState(State targetState)
        {
            if (CurrentState == targetState) throw new InvalidOperationException($"State is already {targetState}");
            
            await _asyncExecutor.EachFrame(TransitionDurationSeconds, t =>
            {
                t = targetState == State.Clean ? t : 1f - t;
                SetOverlayTransparency(t);
            });
            SetState(targetState);
        }

        private void SetOverlayTransparency(float transparency)
        {
            transparency = Mathf.Clamp01(transparency);
            
            _cloudOverlayMaterial.SetFloat(_transparencyShaderProperty, transparency);
        }
    }
}