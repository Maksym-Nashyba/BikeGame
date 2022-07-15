using System;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public class Persistency : IDisposable
    {
        public SaveData Current { get; private set; }
        private IPersistencyProvider _persistencyProvider;

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