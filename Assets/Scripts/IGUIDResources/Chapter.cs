using System;
using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newChapterFile", menuName = "ScriptableObjects/Levels/Chapter")]
    public class Chapter : ScriptableObject
    {
        public Level[] Levels = Array.Empty<Level>();
        public int Count => Levels.Length;
        
        public Level this[int i]
        {
            get => Levels[i];
            set => Levels[i] = value;
        }
    }
}