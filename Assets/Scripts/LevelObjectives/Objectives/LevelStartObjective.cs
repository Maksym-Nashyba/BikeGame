using GameCycle;
using Misc;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    public class LevelStartObjective: Objective
    {
        [SerializeField]private Transform _spawnPoint;

        public override Transformation GetSpawnPosition()
        {
            return new Transformation(_spawnPoint.transform);
        }

        public override void Begin(LevelAchievements levelAchievements)
        {
            base.Begin(levelAchievements);
            End();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.identity * Matrix4x4.Translate(_spawnPoint.position) * Matrix4x4.Rotate(_spawnPoint.rotation);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(Vector3.zero, _spawnPoint.localScale);
        }
    #endif
    }
}