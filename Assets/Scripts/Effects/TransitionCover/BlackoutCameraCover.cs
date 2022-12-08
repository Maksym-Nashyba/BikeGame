using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Effects.TransitionCover
{
    public class BlackoutCameraCover : SceneTransitionCover
    {
        [SerializeField] private CanvasGroup _cover;

        protected override async Task PlayTransitionAnimation(State targetState)
        {
            _cover.gameObject.SetActive(true);
            await AsyncExecutor.EachFrame(TransitionDurationSeconds, t =>
            {
                t = targetState == State.Covered ? t : 1f - t;
                _cover.alpha = t;
            }, EaseFunctions.InOutQuad);
            _cover.interactable = false;
            _cover.gameObject.SetActive(targetState == State.Covered);
        }

        protected override void PlayTransitionImmediate(State targetState)
        {
            float t = targetState == State.Clean ? 1f : 0f;
            _cover.alpha = t;
            _cover.interactable = false;
            _cover.gameObject.SetActive(targetState == State.Covered);
        }
    }
}