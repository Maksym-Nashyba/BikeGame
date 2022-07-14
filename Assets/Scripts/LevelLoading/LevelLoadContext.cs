using UnityEngine;

namespace LevelLoading
{
    public class LevelLoadContext
    {
        public readonly string SceneName;
        public readonly GameObject BikePrefab;
        public readonly Material Skin;

        public LevelLoadContext(string sceneName, GameObject bikePrefab, Material skin)
        {
            SceneName = sceneName;
            BikePrefab = bikePrefab;
            Skin = skin;
        }
    }
}