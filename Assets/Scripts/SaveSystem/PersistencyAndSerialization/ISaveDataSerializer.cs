using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public interface ISaveDataSerializer
    {
        public Task<bool> TrySerialize(SaveData saveData, out string serializedData);
        public Task<bool> TryDeserialize(string serializedData, out SaveData saveData);
        public byte GetVersion();
    }
}