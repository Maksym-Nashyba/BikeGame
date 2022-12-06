using System.Collections.Generic;
using GameCycle;
using IGUIDResources;
using LevelLoading;
using LevelObjectives.Objectives;
using UnityEngine;

namespace LevelObjectives
{
    public class LevelStructure : MonoBehaviour
    {
        public Level Level { get; private set; }
        public bool PedalCollected { get; private set; }
        
        public Queue<Objective> ObjectiveQueue
        {
            get => _objectivesQueue.ToQueue();
            private set => ObjectiveQueue = value;
        }
        [SerializeField] private ObjectivesQueue _objectivesQueue;
        
        public void SetUp(LevelLoadContext context)
        {
            Level = context.Level;
            PedalCollected = ((CareerLevelLoadContext)context).PedalCollected;
        }
        
        internal virtual LevelAchievements InstantiateAchievements()
        {
            return new CareerLevelAchievements(Level.ExpectedTimeSeconds);
        }
    }
}