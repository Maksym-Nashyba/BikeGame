using System;
using GameCycle;
using LevelObjectives.LevelObjects;
using Misc;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    [Serializable]
    public class CheckpointObjective : Objective
    {
        [SerializeField]private Checkpoint _checkpoint;

        public override Transformation GetSpawnPosition()
        {
            return new Transformation(_checkpoint.transform);
        }

        public override void Begin(LevelAchievements levelAchievements)
        {
            base.Begin(levelAchievements);
            _checkpoint.Activated += End;
        }

        private void OnDestroy()
        {
            _checkpoint.Activated -= End;
        }
    }
}