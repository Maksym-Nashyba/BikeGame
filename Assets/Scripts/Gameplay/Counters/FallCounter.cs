using GameCycle;
using Misc;
using UnityEngine;

namespace Gameplay.Counters
{
    public class FallCounter : MonoBehaviour
    {
        private int _fallCount;
        
        private void Awake()
        {
            ServiceLocator.Player.Died += OnPlayerDied;
            ServiceLocator.GameLoop.LevelAchievements.CountingScore += OnCountingScore;
        }

        private void OnDestroy()
        {
            ServiceLocator.Player.Died -= OnPlayerDied;
            ServiceLocator.GameLoop.LevelAchievements.CountingScore -= OnCountingScore;
        }

        private void OnPlayerDied()
        {
            _fallCount++;
        }
        
        private void OnCountingScore(LevelAchievements achievements)
        {
            achievements.FallCount = _fallCount;
        }
    }
}