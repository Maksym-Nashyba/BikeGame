using System;
using System.Linq;
using IGUIDResources;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace SaveSystem.Front
{
    public class SavedCareer
    {
        private Persistency _persistency;
        private GUIDResourceLocator _resources;
        private SaveData Save => Persistency.Current;

        public SavedCareer(Persistency persistency, GUIDResourceLocator resources)
        {
            _persistency = persistency;
            _resources = resources;
        }

        public bool IsCompleted(Level level)
        {
            return IsCompleted(level.GetGUID());
        }
        
        public bool IsCompleted(string guid)
        {
            return Save.CareerLevels.Any(level => level.GUID == guid);
        }

        public void SetLevelCompleted(string levelGUID)
        {
            if (IsCompleted(levelGUID)) return;
            Array.Resize(ref Save.CareerLevels, Save.CareerLevels.Length+1);
            Save.CareerLevels[^1] = PersistentLevel.GetNewLevelWithGUID(levelGUID);
            _persistency.Push();
        }

        public float GetBestTime(Level level)
        {
            return GetBestTime(level.GetGUID());
        }

        public float GetBestTime(string levelGUID)
        {
            AssertLevelCompleted(levelGUID);
            PersistentLevel levelSave = GetLevelWithGUID(levelGUID);
            return levelSave.BestTime;
        }

        public void UpdateBestTime(Level level, float newBestTime)
        {
            UpdateBestTime(level.GetGUID(), newBestTime);
        }
        
        public void UpdateBestTime(string levelGUID, float newBestTime)
        {
            AssertLevelCompleted(levelGUID);
            PersistentLevel levelSave = GetLevelWithGUID(levelGUID);
            if (newBestTime >= levelSave.BestTime) return;
            levelSave.BestTime = newBestTime;
            _persistency.Push();
        }

        public bool IsPedalCollected(Level level)
        {
            return IsPedalCollected(level.GetGUID());
        }
        
        public bool IsPedalCollected(string levelGUID)
        {
            AssertLevelCompleted(levelGUID);
            return GetLevelWithGUID(levelGUID).PedalCollected;
        }

        public void SetPedalCollected(Level level)
        {
            SetPedalCollected(level.GetGUID());
        }
        
        public void SetPedalCollected(string levelGUID)
        {
            AssertLevelCompleted(levelGUID);
            PersistentLevel levelSave = GetLevelWithGUID(levelGUID);
            if (levelSave.PedalCollected) return;
            _persistency.Push();
        }
        
        private void AssertLevelCompleted(string levelGUID)
        {
            if (!IsCompleted(levelGUID)) throw new Exception($"Level is not completed. GUID:{levelGUID}");
        }
        
        private PersistentLevel GetLevelWithGUID(string guid)
        {
            return Save.CareerLevels.First(level => level.GUID == guid);
        }
    }
}