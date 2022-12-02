using GameCycle;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        private void OnDisable()
        {
            _gameLoop.Ended -= ShowEndGameScreen;
        }

        private async void ShowEndGameScreen(LevelAchievements achievements)
        {
            _endGameScreen.SetActive(true);
            _controlls.SetActive(false);
            await endGameScreen.Show(achievements);
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

        public void OnMenuButton()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        private void HidePauseMenu()
        {
            _pauseScreen.SetActive(false);
        }

        private void DisplayPauseMenu()
        {
            _pauseScreen.SetActive(true);
        }
    }
}