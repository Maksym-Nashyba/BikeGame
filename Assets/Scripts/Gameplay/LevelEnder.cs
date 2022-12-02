using Misc;
using UnityEngine;

namespace GameCycle
{
    public class LevelEnder : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.GameLoop.Ended += OnLevelComplete;
        }

        protected virtual void OnLevelComplete(LevelAchievements achievements)
        {
            ServiceLocator.Saves.Currencies.AddDollans((long)achievements.ScoreBonus);
        }

        private void OnDisable()
        {
            ServiceLocator.GameLoop.Ended -= OnLevelComplete;
        }
    }
}