using Gameplay;
using Misc;

namespace Effects.TransitionCover
{
    public class LevelCloudCover : CameraCloudCover
    {
        protected override void Awake()
        {
            base.Awake();
            TransitionOnStart = false;
            ServiceLocator.GameLoop.IntroPhase.SubscribeAwaited(async () =>
            {
                await TransitionToState(State.Clean);
            });
        }
    }
}