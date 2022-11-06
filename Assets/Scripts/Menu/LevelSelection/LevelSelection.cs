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
        private PersistentLevel[] _completedLevels;
        private GUIDResourceLocator _resourceLocator;
        private int LastUnlockedLevelIndex => _completedLevels.Length - 1;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _completedLevels = _saves.Career.GetAllCompletedLevels();
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
            await loader.LoadLevelWithBikeSelection(_career.Chapters[0][CurrentLevelIndex].GetGUID());
        }

        public bool CanSelectNext()
        {
            return CurrentLevelIndex < LastUnlockedLevelIndex;
        }

        public bool CanSelectPrevious()
        {
            return CurrentLevelIndex > 0;
        }
    }
}