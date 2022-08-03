using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using SaveSystem.Models;

namespace SaveSystem.PersistencyAndSerialization
{
    public class BinarySaveDataSerializer : ISaveDataSerializer
    {
        public byte GetVersion() => 1;
        public byte GetSerializerIndex() => 1;

        public Task<bool> TrySerialize(SaveData saveData, out byte[] serializedData)
        {
            if (GetVersion() != saveData.Version) throw new Exception("Data version doesn't match serializer version");
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            stream.WriteByte(GetSerializerIndex());
            stream.WriteByte(GetVersion());
            formatter.Serialize(stream, saveData);
            serializedData = stream.ToArray();
            stream.Close();
            return Task.FromResult(true);
        }

        public Task<bool> TryDeserialize(byte[] serializedData, out SaveData saveData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(serializedData.Length - 2);
            new BinaryWriter(stream).Write(serializedData, 2, serializedData.Length - 2);
            saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return Task.FromResult(true);
        }
    }
}