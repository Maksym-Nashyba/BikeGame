using System;
using GameCycle;
using Misc;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    public abstract class Objective : MonoBehaviour
    {
        public event Action<Objective> Completed;
        private LevelAchievements _levelAchievements;

        public abstract Transformation GetSpawnPosition();
        
        public virtual void Begin(LevelAchievements levelAchievements)
        {
            _levelAchievements = levelAchievements;
        }
        
        protected void End()
        {
            OnLevelEnd();
            Completed?.Invoke(this);
        }

        protected virtual void OnLevelEnd() { }
    }
}