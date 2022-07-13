using UnityEngine;

namespace LevelLoading
{
    public class CareerLevelLoadContext : LevelLoadContext
    {
        public readonly bool PedalCollected;
        
        public CareerLevelLoadContext(string sceneName, GameObject prefab, Material skin, bool pedalCollected) : base(sceneName, prefab, skin)
        {
            PedalCollected = pedalCollected;
        }
    }
}