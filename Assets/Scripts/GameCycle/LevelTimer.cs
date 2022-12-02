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
            ServiceLocator.GameLoop.LevelAchievements.CountingScore += OnCountingScore;
        }
        
        private void OnDestroy()
        {
            ServiceLocator.GameLoop.LevelAchievements.CountingScore -= OnCountingScore;
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
        
        private void OnCountingScore(LevelAchievements levelAchievements)
        {
            levelAchievements.TimeSeconds = (int)TimePassed;
        }
    }
}