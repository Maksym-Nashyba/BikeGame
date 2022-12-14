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
        public GameObject PlayerPrefab { get; private set; }
        public Skin Skin { get; private set; }
        
        public Queue<Objective> ObjectiveQueue
        {
            get => _objectivesQueue.ToQueue();
            private set => ObjectiveQueue = value;
        }
        [SerializeField] private ObjectivesQueue _objectivesQueue;

        private void Awake()
        {
            SetUp(LevelContextContainer.Consume());
        }

        public void SetUp(LevelLoadContext context)
        {
            Level = context.Level;
            PedalCollected = ((CareerLevelLoadContext)context).PedalCollected;
            PlayerPrefab = context.BikePrefab;
            Skin = context.Skin;
        }
        
        internal virtual LevelAchievements InstantiateAchievements()
        {
            return new CareerLevelAchievements(Level.ExpectedTimeSeconds);
        }
    }
}