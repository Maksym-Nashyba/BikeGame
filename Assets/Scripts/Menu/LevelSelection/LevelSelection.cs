using System;
using IGUIDResources;
using LevelLoading;
using SaveSystem.Front;
using UnityEngine;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public int CurrentLevelIndex { get; private set; }
        public int LastCompletedLevelIndex => _saves.Career.GetAllCompletedLevels().Length - 1;
        public int SafeLastCompletedLevelIndex => Mathf.Clamp(_saves.Career.GetAllCompletedLevels().Length - 1, 0, Int32.MaxValue);
        public int NextSafeIndex(int levelIndex) => Mathf.Clamp(levelIndex + 1, 0, _career.Chapters[0].Count - 1);
        public event Action<int, Level> SelectedLevel;
        public event Action<int, Level> SetUp;

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
        
        public async void LaunchLevel()
        {
            LevelLoader loader = new LevelLoader();
            await loader.LoadLevelWithBikeSelection(CurrentLevelGUID);
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
            int currentLevelComplete = _saves.Career.IsCompleted(CurrentLevelGUID) ? 1 : 0;
            return CurrentLevelIndex < LastCompletedLevelIndex + currentLevelComplete;
        }

        public bool CanSelectPrevious()
        {
            return CurrentLevelIndex > 0;
        }
    }
}