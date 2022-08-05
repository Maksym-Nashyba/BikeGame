using System;
using System.Threading.Tasks;
using IGUIDResources;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;
using UnityEngine;

namespace SaveSystem.Front
{
    public class Saves : MonoBehaviour
    {
        public event Action Initialized;
        public SavedBikes Bikes { get; private set; }
        public SavedCurrencies Currencies { get; private set; }
        public SavedCareer Career { get; private set; }
        
        private IPersistencyProvider<ISaveDataSerializer> _persistencyProvider;
        private SaveData _currentData;
        private bool _isValid = false;
        
        public async void Initialize(IPersistencyProvider<ISaveDataSerializer> persistencyProvider)
        {
            GUIDResourceLocator resourceLocator = GUIDResourceLocator.Initialize();
            InitializeFacades(resourceLocator);
            
            _persistencyProvider = persistencyProvider;
            
            await Pull();
            _isValid = true;
            
            Initialized?.Invoke();
        }

        private void InitializeFacades(GUIDResourceLocator resourceLocator)
        {
            Bikes = new SavedBikes(_currentData, resourceLocator);
            Career = new SavedCareer(_currentData, resourceLocator);
            Currencies = new SavedCurrencies(_currentData);

            Bikes.Changed += Push;
            Career.Changed += Push;
            Currencies.Changed += Push;
        }

        public async void Push()
        {
            if (_isValid == false)
            {
                throw new InvalidOperationException($"Class saves should be initialize with {nameof(SavesInitializer)}");
            }
            await _persistencyProvider.Save(_currentData);
        }

        public async Task Pull()
        {
            if (_isValid == false)
            {
                throw new InvalidOperationException($"Class saves should be initialize with {nameof(SavesInitializer)}");
            }
            _currentData = await _persistencyProvider.Load();
        }

        private void OnDestroy()
        {
            _persistencyProvider.CancelAllOperations();
            
            Bikes.Changed -= Push;
            Career.Changed -= Push;
            Currencies.Changed -= Push;
        }
    }
}