using NUnit.Framework;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace Tests
{
    public class SerializersTests
    {
        [Test]
        public void BinarySerializeDeserialize()
        {
            SaveData originalData = MockData.GenerateMockSaveData();
            BinarySaveDataSerializer serializer = new BinarySaveDataSerializer();
            serializer.TrySerialize(originalData, out byte[] serializedData);
            serializer.TryDeserialize(serializedData, out SaveData deserializedData);
            Assert.True(Equals(originalData, deserializedData));
        }
    }
}