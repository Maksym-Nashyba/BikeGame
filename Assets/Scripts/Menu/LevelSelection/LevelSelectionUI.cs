using System;
using IGUIDResources;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [Space]
        [SerializeField] private LevelSelection _levelSelection; 
        [SerializeField] private LevelSelectionCamera _selectionCamera;

        private void Awake()
        {
            _levelSelection.SelectedLevel += OnSelectedLevel;
            _levelSelection.SetUp += OnSelectionSetUp;
        }

        private void OnSelectionSetUp(int levelIndex, Level level)
        {
            _levelSelection.SetUp -= OnSelectionSetUp;
            
            _selectionCamera.TeleportToLevel(levelIndex);
            EnableUI(level);
        }
        
        private async void OnSelectedLevel(int levelIndex, Level level)
        {
            DisableUI();
            
            await _selectionCamera.MoveToLevel(levelIndex);
            EnableUI(level);
        }

        private void DisableUI()
        {
            _nextButton.interactable = false;
            _previousButton.interactable = false;
        }

        private void EnableUI(Level level)
        {
            UpdateNavigationButtonsInteractable();
        }

        private void UpdateNavigationButtonsInteractable()
        {
            _nextButton.interactable = _levelSelection.CanSelectNext();
            _previousButton.interactable = _levelSelection.CanSelectPrevious();
        }
        
        private void OnDestroy()
        {
            _levelSelection.SelectedLevel -= OnSelectedLevel;
        }
    }
}