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
    }
}