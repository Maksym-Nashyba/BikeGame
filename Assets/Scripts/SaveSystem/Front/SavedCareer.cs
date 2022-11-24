using System;
using System.Linq;
using IGUIDResources;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace SaveSystem.Front
{
    public class SavedCareer
    {
        public event Action Changed;
        private GUIDResourceLocator _resources;
        private SaveData _saveData;

        public SavedCareer(SaveData saveData, GUIDResourceLocator resources)
        {
            UpdateData(saveData);
            _resources = resources;
        }

        private void UpdateData(SaveData saveData)
        {
            _saveData = saveData;
        }

        public PersistentLevel[] GetAllCompletedLevels()
        {
            return _saveData.CareerLevels;
        }
        
        public bool IsCompleted(Level level)
        {
            return IsCompleted(level.GetGUID());
        }
        
        public bool IsCompleted(string guid)
        {
            return _saveData.CareerLevels.Any(level => level.GUID == guid);
        }

        public void SetLevelCompleted(string levelGUID)
        {
            if (IsCompleted(levelGUID)) return;
            Array.Resize(ref _saveData.CareerLevels, _saveData.CareerLevels.Length+1);
            _saveData.CareerLevels[^1] = PersistentLevel.GetNewLevelWithGUID(levelGUID);
            Changed?.Invoke();
        }

        public int GetBestTime(Level level)
        {
            return GetBestTime(level.GetGUID());
        }

        public int GetBestTime(string levelGUID)
        {
            AssertLevelCompleted(levelGUID);
            PersistentLevel levelSave = GetLevelWithGUID(levelGUID);
            return levelSave.BestTime;
        }

        public void UpdateBestTime(Level level, int newBestTime)
        {
            UpdateBestTime(level.GetGUID(), newBestTime);
        }
        
        public void UpdateBestTime(string levelGUID, int newBestTime)
        {
            AssertLevelCompleted(levelGUID);
            PersistentLevel levelSave = GetLevelWithGUID(levelGUID);
            if (newBestTime >= levelSave.BestTime && levelSave.BestTime > 0) return;
            levelSave.BestTime = newBestTime;
            Changed?.Invoke();
        }

        public bool IsPedalCollected(Level level)
        {
            return IsPedalCollected(level.GetGUID());
        }
        
        public bool IsPedalCollected(string levelGUID)
        {
            if (!IsCompleted(levelGUID)) return false;
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
            levelSave.PedalCollected = true;
            Changed?.Invoke();
        }
        
        private void AssertLevelCompleted(string levelGUID)
        {
            if (!IsCompleted(levelGUID)) throw new Exception($"Level is not completed. GUID:{levelGUID}");
        }
        
        private PersistentLevel GetLevelWithGUID(string guid)
        {
            return _saveData.CareerLevels.First(level => level.GUID == guid);
        }
    }
}