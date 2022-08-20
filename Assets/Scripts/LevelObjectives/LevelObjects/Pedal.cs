using System;
using GameCycle;
using Misc;
using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    public class Pedal : MonoBehaviour
    {
        public event Action PedalPickedUp;
        public bool IsHeld { get; private set; }
        [SerializeField] private GameObject _pedalTriggerObject;

        private void Start()
        {
            if (IsAlreadyCollected())
            {
                _pedalTriggerObject.SetActive(false);
            }

            _pedalTriggerObject.GetComponent<PlayerTrigger>().Activated += OnPedalTriggered;
            ServiceLocator.GameLoop.Ended += OnLevelEnded;
        }

        private bool IsAlreadyCollected()
        {
            return ServiceLocator.Saves.Career.IsPedalCollected(ServiceLocator.LevelStructure.Level);
        }

        private void OnPedalTriggered()
        {
            PedalPickedUp?.Invoke();
            _pedalTriggerObject.SetActive(false);
            IsHeld = true;
        }

        private void OnLevelEnded(LevelAchievements achievements)
        {
            if(achievements is not CareerLevelAchievements careerLevelAchievements)return;
            careerLevelAchievements.IsPedalCollected = IsHeld;
        }
    }
}