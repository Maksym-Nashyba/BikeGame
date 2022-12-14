using IGUIDResources;
using UnityEngine;

namespace LevelLoading
{
    public class LevelLoadContext
    {
        public readonly string SceneName;
        public readonly GameObject BikePrefab;
        public readonly Skin Skin;
        public readonly Level Level;

        public LevelLoadContext(string sceneName, GameObject bikePrefab, Skin skin, Level level)
        {
            SceneName = sceneName;
            BikePrefab = bikePrefab;
            Skin = skin;
            Level = level;
        }
    }
}