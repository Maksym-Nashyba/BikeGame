﻿using System.Threading.Tasks;
using UnityEngine;

namespace LevelLoading
{
    public class CameraCloudCover : SceneTransitionCover
    {
        [SerializeField] private Material _cloudOverlayMaterial;
        
        private readonly int _transparencyShaderProperty = Shader.PropertyToID("_Transparency");

        protected override Task PlayTransitionAnimation(State targetState)
        {
            return AsyncExecutor.EachFrame(TransitionDurationSeconds, t =>
            {
                t = targetState == State.Clean ? t : 1f - t;
                SetOverlayTransparency(t);
            });
        }

        protected override void PlayTransitionImmediate(State targetState)
        {
            float t = targetState == State.Clean ? 0f : 1f;
            SetOverlayTransparency(t);
        }

        private void SetOverlayTransparency(float transparency)
        {
            transparency = Mathf.Clamp01(transparency);
            
            _cloudOverlayMaterial.SetFloat(_transparencyShaderProperty, transparency);
        }
    }
}