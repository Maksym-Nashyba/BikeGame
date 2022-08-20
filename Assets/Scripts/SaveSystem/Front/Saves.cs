using System;
using System.Collections.Generic;
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
        public bool IsValid { get; private set; }
        public bool IsSaving { get; private set; }
        public SavedBikes Bikes { get; private set; }
        public SavedCurrencies Currencies { get; private set; }
        public SavedCareer Career { get; private set; }
        
        private IPersistencyProvider<ISaveDataSerializer> _persistencyProvider;
        private SaveData _currentData;
        private Queue<SaveData> _pushQueue;

        #region Initialization

        public async void Initialize(IPersistencyProvider<ISaveDataSerializer> persistencyProvider)
        {
            _persistencyProvider = persistencyProvider;
            _pushQueue = new Queue<SaveData>();
            
            GUIDResourceLocator resourceLocator = GUIDResourceLocator.Initialize();
            await EnsureSaveExists();
            _currentData = await _persistencyProvider.Load();
            InitializeFacades(resourceLocator);
            
            IsValid = true;
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
            if (!IsValid) throw new InvalidOperationException($"Class saves should be initialize with {nameof(SavesInitializer)}");
            
            _currentData = await _persistencyProvider.Load();
        }
        
        private async Task Push(SaveData saveData)
        {
            if (!IsValid) throw new InvalidOperationException($"Class {nameof(Saves)} should be initialized with {nameof(SavesInitializer)}");
            
            _pushQueue.Enqueue(saveData.MakeDeepCopy());

            if (IsSaving) return;
            IsSaving = true;
            while (_pushQueue.Count > 0)
            {
                await _persistencyProvider.Save(_pushQueue.Dequeue());
            }

            IsSaving = false;
        }

        public void ExecuteWhenReady(Action action)
        {
            if (IsValid)
            {
                action.Invoke();
            }
            else
            {
                Initialized += action;
            }
        }
        
        public void ClearSaves()
        {
            _currentData = SaveData.GetDefault();
            Push();
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