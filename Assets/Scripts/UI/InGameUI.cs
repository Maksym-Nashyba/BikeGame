using GameCycle;
using Misc;
using UnityEngine;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        public GameObject JoystickObject;
        [SerializeField] private GameObject _endGameScreen;
        [SerializeField] private GameObject _pauseScreen;
        [SerializeField] private GameObject _controlls;
        [SerializeField] private EndGameScreen endGameScreen;
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

        private async void ShowEndGameScreen(LevelAchievements levelAchievements)
        {
            _endGameScreen.SetActive(true);
            _controlls.SetActive(false);
            await endGameScreen.Show((CareerLevelAchievements)levelAchievements);
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

        public void OnRespawnButton()
        {
            ServiceLocator.Player.Die();
            OnUnpauseButton();
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