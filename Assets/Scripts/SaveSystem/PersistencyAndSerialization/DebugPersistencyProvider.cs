using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public class DebugPersistencyProvider : IPersistencyProvider<BinarySaveDataSerializer>
    {
        public Task Save(SaveData toSave)
        {
            return Task.CompletedTask;
        }

        public Task<SaveData> Load()
        {
            return Task.FromResult(SaveData.GetDefault());
        }

        public Task<bool> SaveExists()
        {
            return Task.FromResult(true);
        }

        public void CancelAllOperations()
        {
            
        }
    }
}