using System;
using GameLoop;

namespace LevelObjectives.Objectives
{
    public class CheckpointObjective : Objective
    {
        
        public event Action<Objective> Completed;
        private Checkpoint _checkpoint;

        public override void Start(LevelAchievements levelAchievements)
        {
            _checkpoint.Activated += InvokeCompleted;
        }

        private void InvokeCompleted()
        {
            Completed?.Invoke(this);
        }
    }
}