using System;

namespace SaveSystem.Models
{
    [Serializable]
    public class PersistentBike
    {
        public string SelectedSkinGUID;
        public string[] UnlockedSkins;
        public string GUID;
        
        public PersistentBike(string selectedSkinGUID, string[] unlockedSkins, string guid)
        {
            SelectedSkinGUID = selectedSkinGUID;
            UnlockedSkins = unlockedSkins;
            GUID = guid;
        }

        public override bool Equals(object obj)
        {
            if (obj is not PersistentBike other) return false;
            if (GUID != other.GUID 
                || SelectedSkinGUID != other.SelectedSkinGUID
                || SelectedSkinGUID.Length != other.SelectedSkinGUID.Length) return false;
            for (int i = 0; i < UnlockedSkins.Length; i++)
            {
                if (UnlockedSkins[i] != other.UnlockedSkins[i]) return false;
            }
            return true;
        }
    }
}