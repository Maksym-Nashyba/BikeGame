using System.Collections.Generic;
using GameLoop;
using LevelObjectives.Objectives;
using UnityEngine;

namespace LevelObjectives
{
    public class LevelStructure : MonoBehaviour
    {
        public Queue<Objective> ObjectiveQueue
        {
            get => _objectivesQueue.ToQueue();
            private set => ObjectiveQueue = value;
        }
        
        [SerializeField] private ObjectivesQueue _objectivesQueue;

        internal virtual LevelAchievements InstantiateAchievements()
        {
            return new LevelAchievements();
        }
    }
}