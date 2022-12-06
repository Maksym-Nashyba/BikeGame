using System.Threading.Tasks;
using GameCycle;
using Gameplay;
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
        [SerializeField] private CanvasGroup _controlls;
        [SerializeField] private EndGameScreen endGameScreen;
        private GameLoop _gameLoop;

        private void Awake()
        {
            _gameLoop = ServiceLocator.GameLoop;
            _gameLoop.IntroPhase.SubscribeAwaited(async () => { await AnimateControlsEnable();});
        }

        private void Start()
        {
            DisableControls();
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
            DisableControls();
            await endGameScreen.Show(achievements);
        }

        public void OnPauseButton()
        {
            ServiceLocator.Pause.PauseAll();
            DisableControls();
            DisplayPauseMenu();
        }
        
        public void OnUnpauseButton()
        {
            ServiceLocator.Pause.ContinueAll();
            EnableControls();
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

        private void EnableControls()
        {
            _controlls.alpha = 1f;
            _controlls.interactable = true;
        }

        private void DisableControls()
        {
            _controlls.alpha = 0f;
            _controlls.interactable = false;
        }
        
        private async Task AnimateControlsEnable()
        {
            AsyncExecutor asyncExecutor = new AsyncExecutor();

            await Task.Delay(1000);
            await asyncExecutor.EachFrame(1f, t =>
            {
                _controlls.alpha = t;
            }, EaseFunctions.InOutQuad);
            _controlls.interactable = true;
            
            asyncExecutor.Dispose();
        }
    }
}