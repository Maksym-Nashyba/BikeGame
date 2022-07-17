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
    }
}