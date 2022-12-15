using System.Collections.Generic;

namespace Menu.Garage.Paint.Display
{
    public class PatternAnimator
    {
        private List<PatternAnimation> _animations;

        public PatternAnimator()
        {
            _animations = new List<PatternAnimation>();
        }

        public PatternAnimation Start(PatternAnimation animation)
        {
            _animations.Add(animation);
            return animation;
        }

        public void CancelAll()
        {
            foreach (PatternAnimation animation in _animations)
            {
                animation.Cancel();
            }
            _animations.Clear();
        }
    }
}