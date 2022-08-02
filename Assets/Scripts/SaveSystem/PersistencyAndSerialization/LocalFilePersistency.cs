using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SaveSystem.Models;
using UnityEngine;

namespace SaveSystem.PersistencyAndSerialization
{
    public class LocalFilePersistency<T> : IPersistencyProvider<T> where T : ISaveDataSerializer
    {
        private string SaveFilePath => Application.persistentDataPath+"/Saves/SaveFile.ngr";
        private string TempFilePath => Application.persistentDataPath+"/Saves/TempSaveFile.ngr";
        private ISaveDataSerializer _serializer;
        private CancellationTokenSource _cancellationTokenSource;

        public LocalFilePersistency(ISaveDataSerializer serializer)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _serializer = serializer;
        }

        public async Task Save(SaveData toSave)
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            bool serialized = await _serializer.TrySerialize(toSave, out string serializedData); 
            if (!serialized) throw new SerializationException($"Serializer: {_serializer.GetType()}. SaveFile: {toSave}");
            
            if (cancellationToken.IsCancellationRequested) return;
            await WriteToFile(TempFilePath, serializedData);
            
            if (cancellationToken.IsCancellationRequested) return;
            await ApplyTemporaryFile();
        }

        public async Task<SaveData> Load()
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            string readData = await ReadFromFile(SaveFilePath);
            if (String.IsNullOrWhiteSpace(readData)) throw new IOException("Read data is null, empty or whitespace");
            
            if (cancellationToken.IsCancellationRequested) return null;
            bool deserialized = await _serializer.TryDeserialize(readData, out SaveData data);
            if(!deserialized) throw new SerializationException($"Serializer: {_serializer.GetType()}. SaveFile: {readData}");
            return data;
        }

        private Task ApplyTemporaryFile()
        {
            File.Delete(SaveFilePath);
            File.Move(TempFilePath, SaveFilePath);
            return Task.CompletedTask;
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel(false);
        }

        private Task WriteToFile(string path, string data)
        {
            File.Delete(path);
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(data);
            }
            return Task.CompletedTask;
        }

        private Task<string> ReadFromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"No file found at '{path}'");
            return Task.FromResult(File.ReadAllText(path));
        }
    }
}