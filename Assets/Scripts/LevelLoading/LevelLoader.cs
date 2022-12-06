using System.Threading.Tasks;
using IGUIDResources;
using Menu.BikeSelectionMenu;
using Misc;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
    public class LevelLoader
    {
        private GUIDResourceLocator _resourceLocator;

        public LevelLoader()
        {
            _resourceLocator = GUIDResourceLocator.Initialize();
        }

        public async Task LoadLevelWithBikeSelection(string levelGUID)
        {
            BikeSelection bikeSelection = await BikeSelection.DisplayBikeSelection();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            
            TaskCompletionSource<BikeModel> taskCompletionSource = new TaskCompletionSource<BikeModel>();
            bikeSelection.RegisterTaskCompletionSource(taskCompletionSource);
            BikeModel selectedBike = await taskCompletionSource.Task;
            
            string sceneName = _resourceLocator.Career.GetLevelWithGUID(levelGUID).SceneName;
            
            LevelLoadContext context = new LevelLoadContext(sceneName, selectedBike.Prefab, selectedBike.AllSkins[0].Material);
            LoadLevel(context);
        }
        
        public void LoadLevel(LevelLoadContext context)
        {
            SceneManager.LoadScene(context.SceneName);
            ServiceLocator.LevelStructure
        }
    }
}
