using System;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace SaveSystem.Front
{
    public class SavedCurrencies
    {
        public event Action Changed;
        private SaveData _saveData;

        public SavedCurrencies(SaveData saveData)
        {
            UpdateData(saveData);
        }
        
        private void UpdateData(SaveData saveData)
        {
            _saveData = saveData;
        }

        public long Dollans
        {
            get
            {
                return _saveData.Currencies.Dollans;
            }
            set
            {
                value = Math.Clamp(value, 0, long.MaxValue);
                _saveData.Currencies.Dollans = value;
                Changed?.Invoke();
            }
        }
        
        public long Pedals
        {
            get
            {
                return _saveData.Currencies.Pedals;
            }
            set
            {
                value = Math.Clamp(value, 0, long.MaxValue);
                _saveData.Currencies.Pedals = value;
                Changed?.Invoke();
            }
        }
    }
}