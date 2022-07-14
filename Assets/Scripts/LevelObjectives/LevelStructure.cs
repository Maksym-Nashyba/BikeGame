using System;
using System.Collections.Generic;
using GameLoop;
using LevelObjectives.Objectives;
using UnityEngine;

namespace LevelObjectives
{
    [Serializable]
    public class LevelStructure : MonoBehaviour
    {
        internal Queue<Objective> Objectives;

        private void Awake()
        {
            Objectives = new Queue<Objective>();
        }

        internal LevelAchievements InstantiateAchievements()
        {
            return new LevelAchievements();
        }
    }
}