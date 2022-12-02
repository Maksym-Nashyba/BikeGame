namespace GameCycle
{
    public class CareerLevelAchievements : LevelAchievements
    {
        public bool IsPedalCollected;

        public CareerLevelAchievements(int expectedTimeSeconds) : base(expectedTimeSeconds)
        {
        }
    }
}