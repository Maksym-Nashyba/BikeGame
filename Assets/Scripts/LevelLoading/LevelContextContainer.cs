using System;
using UnityEngine;

namespace LevelLoading
{
    public class LevelContextContainer : MonoBehaviour
    {
        private static LevelContextContainer _instance;
        private LevelLoadContext _data;
        
        public static void Create(LevelLoadContext context)
        {
            if (_instance != null) throw new Exception("Another instance already exists");
            _instance = new GameObject("LevelContextContainer").AddComponent<LevelContextContainer>();
            DontDestroyOnLoad(_instance);
            _instance._data = context;
        }

        public static LevelLoadContext Consume()
        {
            if (_instance == null) throw new Exception("No container exists");
            LevelLoadContext data = _instance._data;
            Destroy(_instance.gameObject);
            return data;
        }
    }
}