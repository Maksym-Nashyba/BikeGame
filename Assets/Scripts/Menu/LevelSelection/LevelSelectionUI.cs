using System;
using IGUIDResources;
using UnityEngine;

namespace Menu
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private LevelSelection _levelSelection; 
        [SerializeField] private LevelSelectionCamera _selectionCamera;

        private void Awake()
        {
            _levelSelection.SelectedLevel += OnSelectedLevel;
        }

        private async void OnSelectedLevel(int levelIndex, Level level)
        {
            DisableUI();
            await _selectionCamera.MoveToLevel(levelIndex);
            EnableUI(level);
        }

        private void DisableUI()
        {
            throw new NotImplementedException();
        }

        private void EnableUI(Level level)
        {
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {
            _levelSelection.SelectedLevel -= OnSelectedLevel;
        }
    }
}