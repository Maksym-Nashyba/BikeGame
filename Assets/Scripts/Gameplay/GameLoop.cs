using System;
using System.Collections.Generic;
using LevelObjectives.Objectives;
using Misc;
using UnityEngine;

namespace GameCycle
{
    [RequireComponent(typeof(LevelTimer))]
    [RequireComponent(typeof(FallCounter))]
    public class GameLoop : MonoBehaviour
    {
        public event Action Started;
        public event Action<LevelAchievements> Ended;
        
        public LevelAchievements LevelAchievements { get; private set; }
        public Objective PreviousObjective { get; private set; } //TODO refactor this shit
        private Queue<Objective> _objectives;

        private void Awake()
        {
            _objectives = ServiceLocator.LevelStructure.ObjectiveQueue;
            LevelAchievements = ServiceLocator.LevelStructure.InstantiateAchievements();
            PreviousObjective = _objectives.Peek();

            ServiceLocator.Player.Died += async () => { await ServiceLocator.Player.Respawn(1000); };
        }

        private void Start()
        {
            ServiceLocator.Player.Respawn(0);
            StartFirstObjective();
        }
        
        private void OnObjectiveComplete(Objective objective)
        {
            RemoveCurrentObjective();
            if (_objectives.Count == 0) CompleteLevel();
            else StartNextObjective(_objectives.Peek());
        }
        
        private void CompleteLevel()
        {
            ServiceLocator.Pause.PauseAll();
            
            LevelAchievements.CountFinalScore();
            Ended?.Invoke(LevelAchievements);
            SaveProgress();
        }
        
        private void StartFirstObjective()
        {
            StartNextObjective(_objectives.Peek());
            Started?.Invoke();
        }
        
        private void StartNextObjective(Objective objective)
        {
            objective.Completed += OnObjectiveComplete;
            objective.Begin(LevelAchievements);
        }

        private void RemoveCurrentObjective()
        {
            Objective dequeuedObjective = _objectives.Dequeue();
            dequeuedObjective.Completed -= OnObjectiveComplete;
            PreviousObjective = dequeuedObjective;
        }

        private void SaveProgress()
        {
            String levelGUID = ServiceLocator.LevelStructure.Level.GetGUID();
            ServiceLocator.Saves.Career.SetLevelCompleted(levelGUID);
            if(((CareerLevelAchievements)LevelAchievements).IsPedalCollected)ServiceLocator.Saves.Career.SetPedalCollected(levelGUID);
            ServiceLocator.Saves.Career.UpdateBestTime(levelGUID, LevelAchievements.TimeSeconds);
            ServiceLocator.Saves.Currencies.AddDollans(LevelAchievements.FinalScore);
        }
    }
}