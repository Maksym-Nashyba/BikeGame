using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public class DebugPersistencyProvider : IPersistencyProvider
    {
        public Task Save(SaveData toSave)
        {
            return Task.CompletedTask;
        }

        public Task<SaveData> Load()
        {
            return Task.FromResult(SaveData.GetDefault());
        }

        public void Cancel()
        {
            
        }
    }
}