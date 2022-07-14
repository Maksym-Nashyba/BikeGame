using System;
using System.Collections.Generic;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;

namespace GameLoop
{
    public class GameLoop : MonoBehaviour
    {
        public event Action Started;
        public event Action<LevelAchievements> Ended;
        private Queue<Objective> _objectives = ServiceLocator.LevelStructure.Objectives;
        private LevelAchievements _levelAchievements = ServiceLocator.LevelStructure.InstantiateAchievements();

        private void Start()
        {
            StartFirstObjective();
        }
        
        private void StartFirstObjective()
        {
            Started?.Invoke();
            StartObjective(_objectives.Peek());
        }
        
        private void StartObjective(Objective objective)
        {
            if (_objectives.Count == 0)
            {
                Ended?.Invoke(_levelAchievements);
                return;
            }
            objective.Completed += OnObjectiveComplete;
            objective.Start(_levelAchievements);
        }

        private void DeQueueObjective(Objective objective)
        {
            objective.Completed -= OnObjectiveComplete;
            _objectives.Dequeue();
        }

        private void OnObjectiveComplete(Objective objective)
        {
            StartObjective(objective);
        }
    }
}