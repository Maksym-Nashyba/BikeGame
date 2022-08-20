using System;
using System.Collections.Generic;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;

namespace GameCycle
{
    public class GameLoop : MonoBehaviour
    {
        public event Action Started;
        public event Action<LevelAchievements> Ended;
        public event Action Died;
        
        private Queue<Objective> _objectives;
        private LevelAchievements _levelAchievements;

        private void Awake()
        {
            _objectives = ServiceLocator.LevelStructure.ObjectiveQueue;
            _levelAchievements = ServiceLocator.LevelStructure.InstantiateAchievements();
        }

        private void Start()
        {
            StartFirstObjective();
        }
        
        private void StartFirstObjective()
        {
            StartNextObjective(_objectives.Peek());
            Started?.Invoke();
        }

        private void OnObjectiveComplete(Objective objective)
        {
            DeQueueObjective();
            if (_objectives.Count == 0)
            {
                Ended?.Invoke(_levelAchievements);
                ServiceLocator.Pause.PauseAll();
                Debug.Log("Ended");
                return;
            }
            StartNextObjective(_objectives.Peek());
        }
        
        private void DeQueueObjective()
        {
            Objective dequeuedObjective = _objectives.Dequeue();
            dequeuedObjective.Completed -= OnObjectiveComplete;
        }
        
        private void StartNextObjective(Objective objective)
        {
            objective.Completed += OnObjectiveComplete;
            objective.Begin(_levelAchievements);
        }
    }
}