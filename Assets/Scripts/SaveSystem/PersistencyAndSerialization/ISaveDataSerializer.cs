using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public interface ISaveDataSerializer
    {
        //First two bytes of any serialization should be SERIALIZER INDEX and VERSION respectively.
        public byte GetVersion(); //Serializer version should match SaveData version
        public byte GetSerializerIndex();
        public Task<bool> TrySerialize(SaveData saveData, out byte[] serializedData);
        public Task<bool> TryDeserialize(byte[] serializedData, out SaveData saveData);
    }
}