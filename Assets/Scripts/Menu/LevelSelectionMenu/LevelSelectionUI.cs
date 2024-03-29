﻿using Effects.TransitionCover;
using IGUIDResources;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private Transform _fogTransform;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private SceneTransitionCover _blackoutTransitionCover;
        [Space]
        [SerializeField] private LevelSelection _levelSelection; 
        [SerializeField] private LevelSelectionCamera _selectionCamera;

        private void Awake()
        {
            _levelSelection.SetUp += OnSelectionSetUp;
            _levelSelection.SelectedLevel += OnSelectedLevel;
        }

        private void OnDestroy()
        {
            _levelSelection.SelectedLevel -= OnSelectedLevel;
        }

        public async void OnBackButton()
        {
            await _blackoutTransitionCover.TransitionToState(SceneTransitionCover.State.Covered);
            SceneManager.LoadScene("MainMenu");
        }
        
        private void OnSelectionSetUp(int levelIndex, Level level)
        {
            _levelSelection.SetUp -= OnSelectionSetUp;

            PositionFogAfterLevel(_levelSelection.NextSafeIndex(_levelSelection.SafeLastCompletedLevelIndex));
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
            _startButton.interactable = false;
        }

        private void EnableUI(Level level)
        {
            UpdateNavigationButtonsInteractable();
            _startButton.interactable = true;
        }

        private void UpdateNavigationButtonsInteractable()
        {
            _nextButton.interactable = _levelSelection.CanSelectNext();
            _previousButton.interactable = _levelSelection.CanSelectPrevious();
        }

        private void PositionFogAfterLevel(int levelIndex)
        {
            float fogY = _selectionCamera.GetCheckpointFor(levelIndex).FogHeight;
            _fogTransform.position = new Vector3(0f, fogY, 0f);
        }
    }
}