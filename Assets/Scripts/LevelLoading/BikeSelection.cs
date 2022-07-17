using System.Threading.Tasks;
using IGUIDResources;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
    public class BikeSelection : MonoBehaviour
    {
        private GUIDResourceLocator _resourceLocator;
        
        private void Awake()
        {
            _resourceLocator = GUIDResourceLocator.Initialize();
        }

        public static async Task<BikeSelection> Display()
        {
            AsyncOperation loadingProcess = SceneManager.LoadSceneAsync("BikeSelection", LoadSceneMode.Additive);
            while (!loadingProcess.isDone)
            {
                await Task.Yield();
            }
            return FindObjectOfType<BikeSelection>();
        }
        
        public Task<BikeModel> RetrieveSelectedBikeModel()
        {
            return Task.FromResult(_resourceLocator.Bikes.GetDefault());
        }
    }
}