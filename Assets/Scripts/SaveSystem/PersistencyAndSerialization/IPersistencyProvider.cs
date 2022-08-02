using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public interface IPersistencyProvider<out T> where T : ISaveDataSerializer
    {
        public Task Save(SaveData toSave);
        public Task<SaveData> Load();

        public void Cancel();
    }
}