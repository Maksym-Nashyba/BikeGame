using System;
using System.Collections.Generic;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCycle
{
    [RequireComponent(typeof(LevelTimer))]
    public class GameLoop : MonoBehaviour
    {
        public event Action Started;
        public event Action<LevelAchievements> Ended;
        public Objective PreviousObjective { get; private set; }
        private Queue<Objective> _objectives;
        private LevelAchievements _levelAchievements;

        private void Awake()
        {
            _objectives = ServiceLocator.LevelStructure.ObjectiveQueue;
            _levelAchievements = ServiceLocator.LevelStructure.InstantiateAchievements();
            PreviousObjective = _objectives.Peek();

            ServiceLocator.Player.Died += async () => { await ServiceLocator.Player.Respawn(1000); };
        }

        private void Start()
        {
            ServiceLocator.Player.Respawn(0);
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
                CompleteLevel();
                return;
            }
            StartNextObjective(_objectives.Peek());
        }

        private void DeQueueObjective()
        {
            Objective dequeuedObjective = _objectives.Dequeue();
            dequeuedObjective.Completed -= OnObjectiveComplete;
            PreviousObjective = dequeuedObjective;
        }
        
        private void StartNextObjective(Objective objective)
        {
            objective.Completed += OnObjectiveComplete;
            objective.Begin(_levelAchievements);
        }

        private void CompleteLevel()
        {
            ServiceLocator.Pause.PauseAll();
            Ended?.Invoke(_levelAchievements);
            try
            {
                String levelGUID = ServiceLocator.LevelStructure.Level.GetGUID();
                ServiceLocator.Saves.Career.SetLevelCompleted(levelGUID);
                if(((CareerLevelAchievements)_levelAchievements).IsPedalCollected)ServiceLocator.Saves.Career.SetPedalCollected(levelGUID);
                ServiceLocator.Saves.Career.UpdateBestTime(levelGUID, _levelAchievements.PlayerPerformanceTime);
            }
            catch (Exception)
            {
                Debug.LogError("Failed to save level completion");
            }

            Debug.Log("Ended");
        }
    }
}