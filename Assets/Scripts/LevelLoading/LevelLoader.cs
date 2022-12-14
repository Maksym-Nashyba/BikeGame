using System.Threading.Tasks;
using IGUIDResources;
using Menu.BikeSelectionMenu;
using SaveSystem.Front;
using UnityEngine;
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
            LevelContextContainer.Create(context);
            SceneManager.LoadSceneAsync(context.SceneName);
        }
        
        public async Task LoadLevelWithBikeSelection(string levelGUID)
        {
            BikeModel selectedBike = await RequestBikeSelection();
            Level level = _resourceLocator.Career.GetLevelWithGUID(levelGUID);
            
            LevelLoadContext context = new CareerLevelLoadContext(level.SceneName,
                selectedBike.Prefab, 
                selectedBike.AllSkins[0], 
                level,
                Object.FindObjectOfType<Saves>().Career.IsPedalCollected(levelGUID));
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
