using IGUIDResources;
using UnityEngine;

namespace LevelLoading
{
    public class CareerLevelLoadContext : LevelLoadContext
    {
        public readonly bool PedalCollected;
        
        public CareerLevelLoadContext(string sceneName, GameObject prefab, Material skin,  Level level, bool pedalCollected) : base(sceneName, prefab, skin, level)
        {
            PedalCollected = pedalCollected;
        }
    }
}