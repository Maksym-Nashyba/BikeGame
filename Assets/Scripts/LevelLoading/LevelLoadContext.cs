using UnityEngine;

namespace LevelLoading
{
    public class LevelLoadContext
    {
        public readonly string SceneName;
        public readonly GameObject Prefab;
        public readonly Material Skin;

        public LevelLoadContext(string sceneName, GameObject prefab, Material skin)
        {
            SceneName = sceneName;
            Prefab = prefab;
            Skin = skin;
        }

    }
}