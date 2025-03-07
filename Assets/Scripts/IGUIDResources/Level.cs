﻿using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newLevel", menuName = "ScriptableObjects/Levels/Level")]
    public class Level : ScriptableObject, IGUIDResource
    {
        public string SceneName;
        public string DisplayName;
        public int ExpectedTimeSeconds;

        public string GetGUID()
        {
            return name;
        }
    }
}