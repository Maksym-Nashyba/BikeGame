using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void OpenLevelSelectingScene()
        {
            LoadScene("LevelSelection");
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