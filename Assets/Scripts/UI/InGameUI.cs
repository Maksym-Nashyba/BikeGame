using GameCycle;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class InGameUI: MonoBehaviour
    {
        public GameObject JoystickObject;
        [SerializeField] private GameObject _endGameScreen;
        [SerializeField] private TextMeshProUGUI _scoreValue;
        [SerializeField] private TextMeshProUGUI _timeValue;
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
            _scoreValue.text = levelAchievements.TotalScore.ToString();
            _timeValue.text = levelAchievements.PlayerPerformanceTime.ToString();
        }

        private void OnDisable()
        {
            _gameLoop.Ended -= ShowEndGameScreen;
        }
    }
}