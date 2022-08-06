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

        #region Initialization

        public async void Initialize(IPersistencyProvider<ISaveDataSerializer> persistencyProvider)
        {
            _persistencyProvider = persistencyProvider;
            
            GUIDResourceLocator resourceLocator = GUIDResourceLocator.Initialize();
            InitializeFacades(resourceLocator);

            await EnsureSaveExists();
            _currentData = await _persistencyProvider.Load();
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

        private async Task EnsureSaveExists()
        {
            bool saveExists = await _persistencyProvider.SaveExists();
            if (!saveExists) await _persistencyProvider.Save(SaveData.GetDefault());
        }

        #endregion
        
        public async void Push()
        {
            await Push(_currentData);
        }
        
        public async Task Pull()
        {
            if (!_isValid) throw new InvalidOperationException($"Class saves should be initialize with {nameof(SavesInitializer)}");
            
            _currentData = await _persistencyProvider.Load();
        }
        
        private async Task Push(SaveData saveData)
        {
            if (!_isValid) throw new InvalidOperationException($"Class saves should be initialize with {nameof(SavesInitializer)}");
            
            await _persistencyProvider.Save(saveData);
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