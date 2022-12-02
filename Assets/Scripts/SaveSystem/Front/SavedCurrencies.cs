using System;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;
using UnityEngine;

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

        public long GetDollans()
        {
            return _saveData.Currencies.Dollans;   
        }

        public void AddDollans(long amount)
        {
            if(amount == 0) return;
            if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Can't add less than one dollan");
            amount = Math.Clamp(amount, 1, long.MaxValue - _saveData.Currencies.Dollans);
            _saveData.Currencies.Dollans += amount;
            Changed?.Invoke();
        }
        
        public void SubtractDollans(long amount)
        {
            if(amount == 0) return;
            if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Can't subtract less than one dollan");
            if (amount > _saveData.Currencies.Dollans) throw new ArgumentOutOfRangeException(nameof(amount), "Can't substract more than there is");
            amount = Math.Clamp(amount, 1, _saveData.Currencies.Dollans);
            _saveData.Currencies.Dollans -= amount;
            Changed?.Invoke();
        }

        public long GetPedals()
        {
            return _saveData.Currencies.Pedals;
        }

        public void AddPedals(long amount)
        {
            if(amount == 0) return;
            if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Can't add less than one pedal");
            amount = Math.Clamp(amount, 1, long.MaxValue - _saveData.Currencies.Pedals);
            _saveData.Currencies.Pedals += amount;
            Changed?.Invoke();
        }
        
        public void SubtractPedals(long amount)
        {
            if(amount == 0) return;
            if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Can't subtract less than one pedal");
            if (amount > _saveData.Currencies.Pedals) throw new ArgumentOutOfRangeException(nameof(amount), "Can't substract more than there is");
            amount = Math.Clamp(amount, 1, _saveData.Currencies.Pedals);
            _saveData.Currencies.Pedals -= amount;
            Changed?.Invoke();
        }
    }
}