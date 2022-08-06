using System;
using System.Collections;
using UnityEngine;

namespace IGUIDResources
{
    [CreateAssetMenu(fileName = "newCareerFile", menuName = "ScriptableObjects/Levels/List")]
    public class Career : ScriptableObject, IEnumerable
    {
        public Chapter[] Chapters;

        public Level GetLevelWithGUID(string guid)
        {
            foreach (Level level in this)
            {
                if (level is null) continue;
                if (level.GetGUID() == guid) return level;
            }

            throw new ArgumentOutOfRangeException(nameof(guid), $"There is no level with this GUID:{guid}");
        }
        
        public IEnumerator GetEnumerator()
        {
            return new CareerEnumerator(Chapters);
        }

        public class CareerEnumerator : IEnumerator
        {
            public object Current => _chapters[_chapterIndex].Levels[_levelIndex];
            private Chapter CurrentChapter => _chapters[_chapterIndex];
            private Chapter[] _chapters;
            private int _chapterIndex;
            private int _levelIndex;

            public CareerEnumerator(Chapter[] chapters)
            {
                _chapters = chapters;
            }

            public bool MoveNext()
            {
                _levelIndex++;
                if (_levelIndex >= CurrentChapter.Count)
                {
                    _levelIndex = 0;
                    _chapterIndex++;
                    if (_chapterIndex >= _chapters.Length)
                    {
                        return false;
                    }
                }
                return true;
            }

            public void Reset()
            {
                _chapterIndex = 0;
                _levelIndex = 0;
            }

        }

        public Level GetFirstLevel()
        {
            foreach (Level level in this)
            {
                if (level is not null) return level;
            }

            throw new Exception("There are not levels");
        }
    }
}