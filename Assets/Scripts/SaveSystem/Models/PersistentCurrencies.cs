using System;

namespace SaveSystem.Models
{
    [Serializable]
    public class PersistentCurrencies
    {
        public long Dollans;
        public long Pedals;

        public PersistentCurrencies(long dollans, long pedals)
        {
            Dollans = dollans;
            Pedals = pedals;
        }

        public override bool Equals(object obj)
        {
            if (obj is not PersistentCurrencies other) return false;
            return Dollans == other.Dollans && Pedals == other.Pedals;
        }
    }
}