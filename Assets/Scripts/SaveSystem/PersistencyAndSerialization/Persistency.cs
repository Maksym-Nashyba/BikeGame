using System;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public class Persistency : IDisposable
    {
        public static SaveData Current { get; private set; }
        private IPersistencyProvider<ISaveDataSerializer> _persistencyProvider;

        public Persistency(IPersistencyProvider<ISaveDataSerializer> persistencyProvider)
        {
            _persistencyProvider = persistencyProvider;
        }

        public async void Push()
        {
            await _persistencyProvider.Save(Current);
        }

        public async void Pull()
        {
            Current = await _persistencyProvider.Load();
        }

        public void Dispose()
        {
            _persistencyProvider.Cancel();
        }
    }
}