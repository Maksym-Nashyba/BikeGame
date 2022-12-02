using System;
using UnityEngine;

namespace GameCycle
{
    public class LevelAchievements
    {
        public event Action<LevelAchievements> CountingScore;
        
        public const int FallCost = 40;
        public const int SecondTimeCost = 5;
        public const int BaseScore = 300;

        public int ExpectedTimeSeconds;
        public int TimeSeconds = -1;
        public int ScoreBonus;
        public int FallCount;
        public int FinalScore;

        public LevelAchievements(int expectedTimeSeconds)
        {
            ExpectedTimeSeconds = expectedTimeSeconds;
        }
        
        public virtual void CountFinalScore()
        {
            CountingScore?.Invoke(this);
            int score = BaseScore;

            float timeDifference = ExpectedTimeSeconds - TimeSeconds;
            score += (int)(timeDifference * SecondTimeCost);
            
            score -= FallCost * FallCount;
            score += ScoreBonus;

            FinalScore = Mathf.Clamp(score, 0, int.MaxValue);
        }
    }
}