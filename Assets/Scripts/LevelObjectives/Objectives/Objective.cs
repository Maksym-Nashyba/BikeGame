using System;
using GameCycle;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    public abstract class Objective : MonoBehaviour
    {
        public event Action<Objective> Completed;
        private LevelAchievements _levelAchievements;
        protected void End()
        {
            OnLevelEnd();
            Completed?.Invoke(this);
            Destroy(this);
        }

        protected virtual void OnLevelEnd() { }

        public virtual void Begin(LevelAchievements levelAchievements)
        {
            _levelAchievements = levelAchievements;
            AddScore();
        }
        
        private void AddScore()
        {
            _levelAchievements.TotalScore += 50;
        }
    }
}