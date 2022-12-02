using System.Collections.Generic;
using GameCycle;
using IGUIDResources;
using LevelObjectives.Objectives;
using UnityEngine;

namespace LevelObjectives
{
    public class LevelStructure : MonoBehaviour
    {

        [SerializeField] private Level _level;
        public Level Level => _level;
        
        public Queue<Objective> ObjectiveQueue
        {
            get => _objectivesQueue.ToQueue();
            private set => ObjectiveQueue = value;
        }
        
        [SerializeField] private ObjectivesQueue _objectivesQueue;

        internal virtual LevelAchievements InstantiateAchievements()
        {
            return new CareerLevelAchievements(_level.ExpectedTimeSeconds);
        }
    }
}