using IGUIDResources;
using UnityEngine;

namespace LevelLoading
{
    class ArcadeLevelLoadContext : LevelLoadContext
    {
        public ArcadeLevelLoadContext(string sceneName, GameObject prefab, Material skin, Level level) : base(sceneName, prefab, skin, level)
        {
        }
    }
}