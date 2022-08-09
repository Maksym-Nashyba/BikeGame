using System;

namespace SaveSystem.Models
{
    [Serializable]
    public class PersistentLevel
    {
        public float BestTime;
        public bool PedalCollected;
        public string GUID;

        private PersistentLevel(float bestTime, bool pedalCollected, string guid)
        {
            BestTime = bestTime;
            PedalCollected = pedalCollected;
            GUID = guid;
        }

        public static PersistentLevel GetNewLevelWithGUID(string guid)
        {
            return new PersistentLevel(0, false, guid);
        }

        public override bool Equals(object obj)
        {
            if (obj is not PersistentLevel other) return false;
            if (Math.Abs(BestTime - other.BestTime) > 0.1f
                || PedalCollected != other.PedalCollected
                || GUID != other.GUID)
                return false;
            return true;
        }
    }
}