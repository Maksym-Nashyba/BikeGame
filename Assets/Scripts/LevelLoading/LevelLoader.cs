using System.Threading.Tasks;
using IGUIDResources;
using Menu.BikeSelectionMenu;
using SaveSystem.Front;
using SaveSystem.Models;
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
            PersistentBike selectedBike = await RequestBikeSelection();
            BikeModel selectedBikeModel = _resourceLocator.Bikes.Get(selectedBike.GUID);
            
            Level level = _resourceLocator.Career.GetLevelWithGUID(levelGUID);
            
            LevelLoadContext context = new CareerLevelLoadContext(level.SceneName,
                selectedBikeModel.Prefab, 
                selectedBikeModel.GetSkinFor(selectedBike.SelectedSkinGUID), 
                level,
                Object.FindObjectOfType<Saves>().Career.IsPedalCollected(levelGUID));
            LoadLevel(context);
        }

        private async Task<PersistentBike> RequestBikeSelection()
        {
            BikeSelection bikeSelection = await BikeSelection.DisplayBikeSelection();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            
            TaskCompletionSource<PersistentBike> taskCompletionSource = new TaskCompletionSource<PersistentBike>();
            bikeSelection.RegisterTaskCompletionSource(taskCompletionSource);
            return await taskCompletionSource.Task;
        }
    }
}
