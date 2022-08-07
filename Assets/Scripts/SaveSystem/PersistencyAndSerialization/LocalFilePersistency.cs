using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
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
        private string DirectoryPath => Application.persistentDataPath + "/Saves";
        private ISaveDataSerializer _serializer;
        private CancellationTokenSource _cancellationTokenSource;

        public LocalFilePersistency(ISaveDataSerializer serializer)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _serializer = serializer;
        }

        public async Task Save(SaveData toSave)
        {
            bool serialized = await _serializer.TrySerialize(toSave, out byte[] serializedData); 
            if (!serialized) throw new SerializationException($"Serializer: {_serializer.GetType()}. SaveFile: {toSave}");
            
            if (IsCancelled()) return;
            await WriteToFile(TempFilePath, serializedData);
            
            if (IsCancelled()) return;
            await ApplyTemporaryFile();
        }

        public async Task<SaveData> Load()
        {
            byte[] readData = await ReadFromFile(SaveFilePath);
            ValidateData(readData);
            
            if (IsCancelled()) return null;
            bool deserialized = await _serializer.TryDeserialize(readData, out SaveData data);
            if(!deserialized) throw new SerializationException($"Serializer: {_serializer.GetType()}. SaveFile: {readData}");
            return data;
        }

        public Task<bool> SaveExists()
        {
            EnsureDirectoryExists();
            return Task.FromResult(File.Exists(SaveFilePath));
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }            
        }
        
        private Task ApplyTemporaryFile()
        {
            File.Delete(SaveFilePath);
            File.Move(TempFilePath, SaveFilePath);
            return Task.CompletedTask;
        }

        public void CancelAllOperations()
        {
            _cancellationTokenSource.Cancel(false);
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private Task WriteToFile(string path, byte[] data)
        {
            File.Create(path).Dispose();
            File.WriteAllBytes(path, data);
            return Task.CompletedTask;
        }

        private Task<byte[]> ReadFromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"No file found at '{path}'");
            return Task.FromResult(File.ReadAllBytes(path));
        }

        private void ValidateData(byte[] data)
        {
            if (data is null || data.Length < 2) throw new DataException("Read data is null or empty");
            if (data[0] != _serializer.GetSerializerIndex()) 
                throw new DataException($"Serializer index ({_serializer.GetSerializerIndex()})" +
                                        $" doesn't match data's serializer index ({data[0]})");
            if (data[1] != _serializer.GetVersion())
                throw new DataException($"Serializer version ({_serializer.GetVersion()})" + 
                                        $" doesn't match data's serializer version ({data[1]})");
        }

        private bool IsCancelled()
        {
            return _cancellationTokenSource.Token.IsCancellationRequested;
        }
    }
}