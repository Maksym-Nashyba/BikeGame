namespace GameCycle
{
    public struct ScoreCount
    {
        public const int FallCost = 40;
        public const int OnePercentTimeCost = 1;
        public const int BaseScore = 300;

        public readonly int ExpectedTimeSeconds;
        public readonly int TimeSeconds;
        public readonly int Falls;
        public readonly int Score;

        public ScoreCount(int timeSeconds, int falls, int score, int expectedTimeSeconds)
        {
            TimeSeconds = timeSeconds;
            Falls = falls;
            Score = score;
            ExpectedTimeSeconds = expectedTimeSeconds;
        }

        public static ScoreCount Calculate(CareerLevelAchievements levelAchievements, int expectedTimeSeconds)
        {
            int score = BaseScore;

            float timeDifference = (expectedTimeSeconds - levelAchievements.TimeSeconds) / (float)expectedTimeSeconds;
            score += (int)(timeDifference * 100f * OnePercentTimeCost);
            
            score -= FallCost * levelAchievements.FallCount;

            return new ScoreCount(
                levelAchievements.TimeSeconds, 
                levelAchievements.FallCount,
                score,
                expectedTimeSeconds);
        }
    }
}