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

        public async void LoadLevel(string levelGUID)
        {
            BikeSelection bikeSelection = BikeSelection.Display();
            BikeModel selectedBike = await bikeSelection.RetrieveSelectedBikeModel();
            string sceneName = _resourceLocator.Career.GetLevelWithGUID(levelGUID).SceneName;
            //LevelLoadContext context = new LevelLoadContext();
        }
        
        public void LoadLevel(LevelLoadContext context)
        {
            SceneManager.LoadScene(context.SceneName);
        }
    }
}
