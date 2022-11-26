namespace GameCycle
{
    public struct ScoreCount
    {
        public const int FallCost = 40;
        public const int OnePercentTimeCost = 1;
        public const int BaseScore = 300;
        
        public readonly int Time;
        public readonly int Falls;
        public readonly int Score;

        public ScoreCount(int time, int falls, int score)
        {
            Time = time;
            Falls = falls;
            Score = score;
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
                score);
        }
    }
}