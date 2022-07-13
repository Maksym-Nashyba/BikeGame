using System.Threading.Tasks;
using IGUIDResources;
using UnityEngine;

namespace LevelLoading
{
    public class BikeSelection : MonoBehaviour
    {
        private GUIDResourceLocator _resourceLocator;
        
        private void Awake()
        {
            _resourceLocator = GUIDResourceLocator.Initialize();
        }

        public static BikeSelection Display()
        {
            return null;//TODO display additive scene
        }
        
        public Task<BikeModel> RetrieveSelectedBikeModel()
        {
            return Task.FromResult(_resourceLocator.Bikes.GetDefault());
        }
    }
}