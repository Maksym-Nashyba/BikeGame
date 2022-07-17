﻿using GameLoop;

namespace LevelObjectives
{
    public class CareerLevelStructure : LevelStructure
    {
        internal override LevelAchievements InstantiateAchievements()
        {
            return new CareerLevelAchievements();
        }
    }
}