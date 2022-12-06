using System.Threading.Tasks;
using IGUIDResources;
using Menu.BikeSelectionMenu;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
    public sealed class LevelLoader
    {
        private readonly GUIDResourceLocator _resourceLocator;

        public LevelLoader()
        {
            _resourceLocator = GUIDResourceLocator.Initialize();
        }

        public void LoadLevel(LevelLoadContext context)
        {
            SceneManager.LoadScene(context.SceneName);
        }
        
        public async Task LoadLevelWithBikeSelection(string levelGUID)
        {
            BikeModel selectedBike = await RequestBikeSelection();
            
            string sceneName = _resourceLocator.Career.GetLevelWithGUID(levelGUID).SceneName;
            
            LevelLoadContext context = new LevelLoadContext(sceneName, selectedBike.Prefab, selectedBike.AllSkins[0].Material, _resourceLocator.Career.GetLevelWithGUID(levelGUID));
            LoadLevel(context);
        }

        private async Task<BikeModel> RequestBikeSelection()
        {
            BikeSelection bikeSelection = await BikeSelection.DisplayBikeSelection();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            
            TaskCompletionSource<BikeModel> taskCompletionSource = new TaskCompletionSource<BikeModel>();
            bikeSelection.RegisterTaskCompletionSource(taskCompletionSource);
            return await taskCompletionSource.Task;
        }
    }
}
