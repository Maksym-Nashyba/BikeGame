using System;
using GameLoop;

namespace LevelObjectives.Objectives
{
    public abstract class Objective
    {
        public event Action<Objective> Completed;
        public abstract void Start(LevelAchievements levelAchievements);

    }
}