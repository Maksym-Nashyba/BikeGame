using UnityEngine;

namespace LevelLoading
{
    class ArcadeLevelLoadContext : LevelLoadContext
    {
        public ArcadeLevelLoadContext(string sceneName, GameObject prefab, Material skin) : base(sceneName, prefab, skin)
        {
        }
    }
}