using System;

namespace SaveSystem.Models
{
    [Serializable]
    public class SaveData
    {
        public PersistentLevel[] CareerLevels;
        public PersistentBike[] Bikes;
        public PersistentCurrencies Currencies;
    }
}