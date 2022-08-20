using System.Globalization;
using GameCycle;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InGameUI: MonoBehaviour
    {
        public GameObject JoystickObject;
        [SerializeField] private GameObject _endGameScreen;
        [SerializeField] private GameObject _pauseScreen;
        [SerializeField] private GameObject _controlls;
        [SerializeField] private TextMeshProUGUI _scoreValue;
        [SerializeField] private TextMeshProUGUI _timeValue;
        [SerializeField] private Toggle _pedalCollectedToggle;
        private GameLoop _gameLoop;

        private void Awake()
        {
            _gameLoop = ServiceLocator.GameLoop;
        }

        private void Start()
        {
            _gameLoop.Ended += ShowEndGameScreen;
            _endGameScreen.SetActive(false);
        }

        private void ShowEndGameScreen(LevelAchievements levelAchievements)
        {
            SetAchievementsValues(levelAchievements);
            _endGameScreen.SetActive(true);
        }

        private void SetAchievementsValues(LevelAchievements levelAchievements)
        {
            _scoreValue.text = levelAchievements.TotalScore.ToString(CultureInfo.InvariantCulture);
            _timeValue.text = levelAchievements.PlayerPerformanceTime.ToString(CultureInfo.InvariantCulture);
            _pedalCollectedToggle.isOn = ((CareerLevelAchievements)levelAchievements).IsPedalCollected;
        }

        public void OnPauseButton()
        {
            ServiceLocator.Pause.PauseAll();
            _controlls.SetActive(false);
            DisplayPauseMenu();
        }
        
        public void OnUnpauseButton()
        {
            ServiceLocator.Pause.ContinueAll();
            _controlls.SetActive(true);
            HidePauseMenu();
        }

        private void HidePauseMenu()
        {
            _pauseScreen.SetActive(false);
        }

        private void DisplayPauseMenu()
        {
            _pauseScreen.SetActive(true);
        }
        
        private void OnDisable()
        {
            _gameLoop.Ended -= ShowEndGameScreen;
        }
    }
}