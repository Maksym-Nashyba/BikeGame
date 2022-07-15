using System;

namespace SaveSystem.Models
{
    [Serializable]
    public class PersistentLevel
    {
        public float BestTime;
        public bool PedalCollected;
        public bool Completed;
        public string GUID;
    }
}