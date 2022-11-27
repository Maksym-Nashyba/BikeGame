using Misc;
using UnityEngine;

namespace GameCycle
{
    public class FallCounter : MonoBehaviour
    {
        private int _fallCount;
        
        private void Awake()
        {
            ServiceLocator.Player.Died += OnPlayerDied;
            ServiceLocator.GameLoop.Ended += OnGameEnded;
        }

        private void OnDestroy()
        {
            ServiceLocator.Player.Died -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            _fallCount++;
        }
        
        private void OnGameEnded(LevelAchievements achievements)
        {
            achievements.FallCount = _fallCount;
        }
    }
}