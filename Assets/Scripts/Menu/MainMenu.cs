using System.Threading.Tasks;
using IGUIDResources;
using LevelLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public async void OpenLevelSelectingScene()
        {
            LevelSelection levelSelection = await LevelSelection.DisplayLevelSelection();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            
            TaskCompletionSource<Level> taskCompletionSource = new TaskCompletionSource<Level>();
            levelSelection.RegisterTaskCompletionSource(taskCompletionSource);
            Level selectedLevel = await taskCompletionSource.Task;

            LevelLoader levelLoader = new LevelLoader();
            await levelLoader.LoadLevelWithBikeSelection(selectedLevel.GetGUID());
        }
    }
}