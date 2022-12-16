using System;
using Effects.TransitionCover;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public event Action SettingsButtonPressed;
        [SerializeField] private SceneTransitionCover _transitionCover;
        
        public void OpenLevelSelectingScene()
        {
            LoadScene("LevelSelection");
        }

        public void OnSettingsButton()
        {
            SettingsButtonPressed?.Invoke();
        }
        
        public async void LoadScene(string sceneName)
        {
            await _transitionCover.TransitionToState(SceneTransitionCover.State.Covered);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
            Debug.Log("Quited");
        }
    }
}