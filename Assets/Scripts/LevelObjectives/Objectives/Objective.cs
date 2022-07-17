using System;
using GameLoop;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    public abstract class Objective : MonoBehaviour
    {
        public event Action<Objective> Completed;
        protected void End()
        {
            OnLevelEnd();
            Completed?.Invoke(this);
            Destroy(this);
        }

        protected virtual void OnLevelEnd() { }
        public abstract void Begin(LevelAchievements levelAchievements);
    }
}