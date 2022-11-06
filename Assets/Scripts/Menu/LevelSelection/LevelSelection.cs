using System;
using IGUIDResources;
using LevelLoading;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public int CurrentLevelIndex { get; private set; }
        public event Action<int, Level> SelectedLevel;
        public event Action<int, Level> SetUp;

        private Saves _saves;
        private Career _career;
        private GUIDResourceLocator _resourceLocator;
        private int LastUnlockedLevelIndex => _saves.Career.GetAllCompletedLevels().Length - 1;
        private String CurrentLevelGUID => _career.Chapters[0][CurrentLevelIndex].GetGUID();

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _resourceLocator = GUIDResourceLocator.Initialize();
            _career = _resourceLocator.Career;
        }

        private void Start()
        {
            CurrentLevelIndex = Mathf.Clamp(LastUnlockedLevelIndex, 0, Int32.MaxValue);
            SetUp?.Invoke(CurrentLevelIndex, _career.Chapters[0][CurrentLevelIndex]);
        }

        public void SelectLevel(int index)
        {
            CurrentLevelIndex = index;
            SelectedLevel?.Invoke(CurrentLevelIndex, _career.Chapters[0][CurrentLevelIndex]);
        }
        
        public void SelectLevel(bool nextOrPrevious) 
        {
            int nextLevelIndex = nextOrPrevious ? ++CurrentLevelIndex : --CurrentLevelIndex;
            SelectLevel(nextLevelIndex);
        }

        public async void LaunchLevel()
        {
            LevelLoader loader = new LevelLoader();
            await loader.LoadLevelWithBikeSelection(CurrentLevelGUID);
        }

        public bool CanSelectNext()
        {
            int currentLevelComplete = _saves.Career.IsCompleted(CurrentLevelGUID) ? 1 : 0;
            return CurrentLevelIndex < LastUnlockedLevelIndex + currentLevelComplete;
        }

        public bool CanSelectPrevious()
        {
            return CurrentLevelIndex > 0;
        }
    }
}