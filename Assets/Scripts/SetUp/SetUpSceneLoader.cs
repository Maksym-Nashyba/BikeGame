using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SetUp
{
    public class SetUpSceneLoader : MonoBehaviour
    {
        [SerializeField] private AudioListener _audioListener;
        private AsyncOperation _sceneLoadingProcess;
        
        private void Awake()
        {
            FindObjectOfType<GameSetUp>().RegisterSetUpTask(ActivateMainMenuSceneWhenLoaded());
        }

        private void Start()
        {
            StartLoadingMainMenu();
        }

        private void StartLoadingMainMenu()
        {
            _sceneLoadingProcess = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            _sceneLoadingProcess.allowSceneActivation = false;
        }

        private async Task ActivateMainMenuSceneWhenLoaded()
        {
            await Task.Delay(5000);
            Destroy(_audioListener);
            while (!_sceneLoadingProcess.isDone)
            {
                await Task.Yield();
                if (_sceneLoadingProcess.progress >= 0.9f)
                {
                    _sceneLoadingProcess.allowSceneActivation = true;
                    SceneManager.UnloadSceneAsync("SetUp");
                }
            }
        }
    }
}