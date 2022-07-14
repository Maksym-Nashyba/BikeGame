using LevelObjectives;
using UnityEngine;

namespace Misc
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Singletron
        {
            get => _serviceLocator;
            private set => _serviceLocator = value;
        }
        private static ServiceLocator _serviceLocator;
        
        public static LevelStructure LevelStructure
        {
            get => _serviceLocator._levelStructure;
        }
        private LevelStructure _levelStructure;
        
        public Camera Camera { get; private set; }

        private void Awake()
        {
            if (_serviceLocator is not null) Destroy(this);
            _serviceLocator = this;
        }
    }
}