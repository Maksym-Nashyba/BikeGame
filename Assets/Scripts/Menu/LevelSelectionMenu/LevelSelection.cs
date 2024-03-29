﻿using System;
using System.Linq;
using System.Threading.Tasks;
using IGUIDResources;
using LevelLoading;
using SaveSystem.Front;
using UnityEngine;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public int CurrentLevelIndex { get; private set; }
        public event Action<int, Level> SelectedLevel;
        public event Action<int, Level> SetUp;
        public event LaunchLayerEffectPlayer LaunchingLevel;
        public delegate Task LaunchLayerEffectPlayer();

        private Saves _saves;
        private Career _career;
        private GUIDResourceLocator _resourceLocator;
        private String CurrentLevelGUID => _career.Chapters[0][CurrentLevelIndex].GetGUID();

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _resourceLocator = GUIDResourceLocator.Initialize();
            _career = _resourceLocator.Career;
        }

        private void Start()
        {
            CurrentLevelIndex = SafeLastCompletedLevelIndex;
            SetUp?.Invoke(CurrentLevelIndex, _career.Chapters[0][CurrentLevelIndex]);
        }

        public int LastCompletedLevelIndex => _saves.Career.GetAllCompletedLevels().Length - 1;
        public int SafeLastCompletedLevelIndex => Mathf.Clamp(_saves.Career.GetAllCompletedLevels().Length - 1, 0, Int32.MaxValue);
        public int NextSafeIndex(int levelIndex) => Mathf.Clamp(levelIndex + 1, 0, _career.Chapters[0].Count - 1);
        
        public async void LaunchLevel()
        {
            LevelLoader loader = new LevelLoader();
            if(LaunchingLevel is not null) await LaunchingLevel.Invoke();
            loader.LoadLevelWithBikeSelection(CurrentLevelGUID);
        }
        
        public void SelectLevel(bool nextOrPrevious)
        {
            int nextLevelIndex = nextOrPrevious ? ++CurrentLevelIndex : --CurrentLevelIndex;
            SelectLevel(nextLevelIndex);
        }
        
        private void SelectLevel(int index)
        {
            CurrentLevelIndex = index;
            SelectedLevel?.Invoke(CurrentLevelIndex, _career.Chapters[0][CurrentLevelIndex]);
        }

        public bool CanSelectNext()
        {
            int accessModifier = _saves.Career.IsCompleted(CurrentLevelGUID) ? 1 : 0;
            accessModifier = _resourceLocator.Career.Cast<Level>().Last().GetGUID() == CurrentLevelGUID ? 0 : accessModifier;
            return CurrentLevelIndex < LastCompletedLevelIndex + accessModifier;
        }

        public bool CanSelectPrevious()
        {
            return CurrentLevelIndex > 0;
        }
    }
}