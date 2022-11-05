using System;
using IGUIDResources;
using LevelLoading;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public event Action<int, Level> SelectedLevel;
        
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;

        private int _currentLevelIndex;
        private Saves _saves;
        private Career _career;
        private PersistentLevel[] _completedLevels;
        private GUIDResourceLocator _resourceLocator;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _completedLevels = _saves.Career.GetAllCompletedLevels();
            _resourceLocator = GUIDResourceLocator.Initialize();
            _career = _resourceLocator.Career;
            SelectedLevel += UpdateNavigationButtonsEnabled;
        }

        private void Start()
        {
            SelectLevel(0);
        }

        private void UpdateNavigationButtonsEnabled(int currentLevelIndex, Level currentLevel)
        {
            _nextButton.interactable = currentLevelIndex < _completedLevels.Length - 1;
            _previousButton.interactable = currentLevelIndex > 0;
        }

        public void SelectLevel(int index)
        {
            _currentLevelIndex = index;
            SelectedLevel?.Invoke(_currentLevelIndex, _career.Chapters[0][_currentLevelIndex]);
        }
        
        public void SelectLevel(bool nextOrPrevious) 
        {
            int nextLevelIndex = nextOrPrevious ? ++_currentLevelIndex : --_currentLevelIndex;
            SelectLevel(nextLevelIndex);
        }

        public async void LaunchLevel()
        {
            LevelLoader loader = new LevelLoader();
            await loader.LoadLevelWithBikeSelection(_career.Chapters[0][_currentLevelIndex].GetGUID());
        }

        private void OnDestroy()
        {
            SelectedLevel -= UpdateNavigationButtonsEnabled;
        }
    }
}