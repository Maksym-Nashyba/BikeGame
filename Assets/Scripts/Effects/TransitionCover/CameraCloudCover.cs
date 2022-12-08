using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Effects.TransitionCover
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
            }, EaseFunctions.InOutQuad);
        }

        protected override void PlayTransitionImmediate(State targetState)
        {
            float t = targetState == State.Clean ? 1f : 0f;
            SetOverlayTransparency(t);
        }

        private void SetOverlayTransparency(float transparency)
        {
            transparency = Mathf.Clamp01(transparency);
            
            _cloudOverlayMaterial.SetFloat(_transparencyShaderProperty, transparency);
        }
    }
}