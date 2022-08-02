using System;
using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public class BinarySaveDataSerializer : ISaveDataSerializer
    {
        public byte GetVersion() => 1;
        public Task<bool> TrySerialize(SaveData saveData, out string serializedData)
        {
            if (GetVersion() != saveData.Version) throw new Exception("Data version doesn't match serializer version");
            throw new NotImplementedException();
        }

        public Task<bool> TryDeserialize(string serializedData, out SaveData saveData)
        {
            throw new NotImplementedException();
        }

    }
}