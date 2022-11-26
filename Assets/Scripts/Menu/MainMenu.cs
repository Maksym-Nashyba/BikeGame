using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public event Action SettingsButtonPressed;
        
        public void OpenLevelSelectingScene()
        {
            LoadScene("LevelSelection");
        }

        public void OnSettingsButton()
        {
            SettingsButtonPressed?.Invoke();
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
            Debug.Log("Quited");
        }
    }
}