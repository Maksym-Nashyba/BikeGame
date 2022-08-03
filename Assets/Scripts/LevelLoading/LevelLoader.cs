using IGUIDResources;
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

        public async void LoadLevelWithBikeSelection(string levelGUID)
        {
            BikeSelection bikeSelection = await BikeSelection.Display();
            BikeModel selectedBike = await bikeSelection.RetrieveSelectedBikeModel();
            
            string sceneName = _resourceLocator.Career.GetLevelWithGUID(levelGUID).SceneName;
            
            LevelLoadContext context = new LevelLoadContext(sceneName, selectedBike.Prefab, selectedBike.AllSkins[0].Material);
            LoadLevel(context);
        }
        
        public void LoadLevel(LevelLoadContext context)
        {
            SceneManager.LoadScene(context.SceneName);
        }
    }
}
