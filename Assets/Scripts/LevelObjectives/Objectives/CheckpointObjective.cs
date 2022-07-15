using System;
using GameLoop;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    [Serializable]
    public class CheckpointObjective : Objective
    {
        
        public event Action<Objective> Completed;
        [SerializeField]private Checkpoint _checkpoint;

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