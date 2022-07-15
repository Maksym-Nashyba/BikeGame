using System.Collections.Generic;
using GameLoop;
using LevelObjectives.Objectives;
using UnityEngine;

namespace LevelObjectives
{
    public class LevelStructure : MonoBehaviour
    {
        public Queue<Objective> ObjectiveQueue { get; private set; }
        [SerializeField] private ObjectivesQueue objectivesQueue;

        private void Awake()
        {
            ObjectiveQueue = objectivesQueue.ToQueue();
        }

        internal virtual LevelAchievements InstantiateAchievements()
        {
            return new LevelAchievements();
        }
    }
}