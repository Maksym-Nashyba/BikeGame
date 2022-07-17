using System;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace SaveSystem.Front
{
    public class SavedCurrencies
    {
        private Persistency _persistency;
        private SaveData Save => Persistency.Current;

        public SavedCurrencies(Persistency persistency)
        {
            _persistency = persistency;
        }
        
        public long Dollans
        {
            get
            {
                return Save.Currencies.Dollans;
            }
            set
            {
                value = Math.Clamp(value, 0, long.MaxValue);
                Save.Currencies.Dollans = value;
                _persistency.Push();
            }
        }
        
        public long Pedals
        {
            get
            {
                return Save.Currencies.Pedals;
            }
            set
            {
                value = Math.Clamp(value, 0, long.MaxValue);
                Save.Currencies.Pedals = value;
                _persistency.Push();
            }
        }
    }
}