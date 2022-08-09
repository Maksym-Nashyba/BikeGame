using NUnit.Framework;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;

namespace Tests.EditMode
{
    public class LocalPersistencyTests
    {
        [Test]
        public void SaveAndLoadData()
        {
            SaveData originalData = MockData.GenerateMockSaveData();
            LocalFilePersistency<ISaveDataSerializer> filePersistency = new LocalFilePersistency<ISaveDataSerializer>(new BinarySaveDataSerializer());
            filePersistency.Save(originalData);
            SaveData loadedSaveData = filePersistency.Load().Result;
            Assert.True(Equals(originalData, loadedSaveData), "Saved data is not equal to loaded data");
        }
    }
}