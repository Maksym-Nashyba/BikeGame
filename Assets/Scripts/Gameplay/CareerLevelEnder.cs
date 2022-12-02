using Misc;

namespace GameCycle
{
    class CareerLevelEnder : LevelEnder
    {
        protected override void OnLevelComplete(LevelAchievements achievements)
        {
            base.OnLevelComplete(achievements);
            if (achievements is not CareerLevelAchievements careerLevelAchievements) return;
            string levelGUID = ServiceLocator.LevelStructure.Level.GetGUID();
            ServiceLocator.Saves.Career.SetLevelCompleted(levelGUID);
            if (careerLevelAchievements.IsPedalCollected)
            {
                ServiceLocator.Saves.Career.SetPedalCollected(levelGUID);
            }
        }
    }
}