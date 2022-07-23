using System;
using System.Threading;
using System.Threading.Tasks;
using SaveSystem.Models;
using UnityEngine;

namespace SaveSystem.PersistencyAndSerialization
{
    public class LocalFilePersistency<ISaveDataSerializer> : IPersistencyProvider<ISaveDataSerializer>
    {
        private CancellationTokenSource _cancellationTokenSource;
        private string SaveFilePath => Application.persistentDataPath+"/Saves/SaveFile.ngr";
        private string TempFilePath => Application.persistentDataPath+"/Saves/TempFile.ngr";
        
        public async Task Save(SaveData toSave)
        {
            
        }

        public async Task<SaveData> Load()
        {
            throw new System.NotImplementedException();
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel(false);
        }

        private Task WriteToFile(string path, string data)
        {
            throw new Exception();
        }
    }
}