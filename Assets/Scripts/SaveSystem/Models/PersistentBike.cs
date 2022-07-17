using System;

namespace SaveSystem.Models
{
    [Serializable]
    public class PersistentBike
    {
        public PersistentBike(bool isBought, string selectedSkinGUID, string[] unlockedSkins, string guid)
        {
            IsBought = isBought;
            SelectedSkinGUID = selectedSkinGUID;
            UnlockedSkins = unlockedSkins;
            GUID = guid;
        }

        public bool IsBought;
        public string SelectedSkinGUID;
        public string[] UnlockedSkins;
        public string GUID;
    }
}