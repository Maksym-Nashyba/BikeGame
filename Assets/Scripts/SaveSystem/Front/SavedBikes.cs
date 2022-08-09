using System;
using System.Collections.Generic;
using System.Linq;
using IGUIDResources;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace SaveSystem.Front
{
    public class SavedBikes
    {
        public event Action Changed;
        private GUIDResourceLocator _resources;
        private SaveData _saveData;
        
        public SavedBikes(SaveData saveData, GUIDResourceLocator resourceLocator)
        {
            UpdateData(saveData);
            _resources = resourceLocator;
        }
        
        private void UpdateData(SaveData saveData)
        {
            _saveData = saveData;
        }

        public PersistentBike WithGUID(string guid)
        {
            return _saveData.Bikes.First(bike => bike.GUID == guid);
        }

        public bool IsSkinUnlocked(Skin skin)
        {
            return IsSkinUnlocked(skin.GetGUID());
        }

        private bool IsSkinUnlocked(string skinGUID)
        {
            foreach (PersistentBike bike in _saveData.Bikes)
            {
                if (!IsBikeUnlocked(bike.GUID)) continue;
                if (bike.UnlockedSkins.Contains(skinGUID)) return true;
            }

            return false;
        }

        public bool IsBikeUnlocked(BikeModel bike)
        {
            return IsBikeUnlocked(bike.GetGUID());
        }

        public bool IsBikeUnlocked(string guid)
        {
            foreach (PersistentBike bike in _saveData.Bikes)
            {
                if (bike.GUID == guid) return true;
            }
            return false;
        }
        
        public Skin GetSelectedSkinFor(BikeModel bike)
        {
            return GetSelectedSkinFor(bike.GetGUID());
        }
        
        public Skin GetSelectedSkinFor(string bikeGUID)
        {
            string skinGUID = WithGUID(bikeGUID).SelectedSkinGUID;
            return _resources.Bikes.Get(bikeGUID).AllSkins
                .First(skin => skin.GetGUID() == skinGUID);
        }
        
        public Skin[] GetUnlockedSkinsFor(BikeModel bike)
        {
            return GetUnlockedSkinsFor(bike.GetGUID());
        }
        
        public Skin[] GetUnlockedSkinsFor(string bikeGUID)
        {
            string[] unlockedGUIDs = WithGUID(bikeGUID).UnlockedSkins;
            Skin[] allSkins = _resources.Bikes.Get(bikeGUID).AllSkins;
            List<Skin> unlockedSkins = new List<Skin>();
            foreach (Skin modelSkin in allSkins)
            {
                if(unlockedGUIDs.Contains(modelSkin.GetGUID()))unlockedSkins.Add(modelSkin);
            }
            return unlockedSkins.ToArray();
        }

        public void UnlockBike(string bikeGUID)
        {
            if (IsBikeUnlocked(bikeGUID)) throw new Exception($"Bike already unlocked. GUID: {bikeGUID}");
            Array.Resize(ref _saveData.Bikes, _saveData.Bikes.Length+1);
            _saveData.Bikes[^1] = _resources.Bikes.Get(bikeGUID).MakeCleanSaveObject();
            Changed?.Invoke();
        }

        public void UnlockSkin(string bikeGUID, string skinGUID)
        {
            if (IsSkinUnlocked(skinGUID)) throw new Exception($"Skin already unlocked. Skin: {skinGUID}");
            PersistentBike targetBike = WithGUID(bikeGUID);
            Array.Resize(ref targetBike.UnlockedSkins, targetBike.UnlockedSkins.Length + 1);
            targetBike.UnlockedSkins[^1] = skinGUID;
            Changed?.Invoke();
        }
    }
}