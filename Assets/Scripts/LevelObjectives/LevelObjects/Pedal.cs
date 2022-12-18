using System;
using GameCycle;
using Gameplay;
using Misc;
using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    public class Pedal : MonoBehaviour
    {
        public event Action<PedalPickedUpArgs> PedalPickedUp;
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

        private bool IsAlreadyCollected() => ServiceLocator.LevelStructure.PedalCollected;

        private void OnPedalTriggered()
        {
            PedalPickedUp?.Invoke(new PedalPickedUpArgs(new Transformation(_pedalTriggerObject.transform)));
            _pedalTriggerObject.SetActive(false);
            IsHeld = true;
        }

        private void OnLevelEnded(LevelAchievements achievements)
        {
            if (achievements is not CareerLevelAchievements careerLevelAchievements) return;
            careerLevelAchievements.IsPedalCollected = IsHeld;
        }

        public class PedalPickedUpArgs
        {
            public readonly Transformation GFXTransformation;

            public PedalPickedUpArgs(Transformation gfxTransformation)
            {
                GFXTransformation = gfxTransformation;
            }
        }
    }
}