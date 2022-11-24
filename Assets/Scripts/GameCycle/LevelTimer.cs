using Misc;
using Pausing;
using UnityEngine;

namespace GameCycle
{
    public class LevelTimer : MonoBehaviour, IPausable
    {
        public float TimePassed { get; private set; }
        private bool _isPaused;
        
        private void Awake()
        {
            ServiceLocator.GameLoop.Ended += OnLevelEnded;
        }
        
        private void OnDestroy()
        {
            ServiceLocator.GameLoop.Ended -= OnLevelEnded;
        }

        private void Update()
        {
            if (_isPaused) return;
            TimePassed += Time.deltaTime;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Continue()
        {
            _isPaused = false;
        }
        
        private void OnLevelEnded(LevelAchievements levelAchievements)
        {
            levelAchievements.PlayerPerformanceTime = (int)TimePassed;
        }
    }
}