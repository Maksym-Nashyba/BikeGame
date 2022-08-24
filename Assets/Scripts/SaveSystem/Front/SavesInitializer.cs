using System;
using System.Threading.Tasks;
using SaveSystem.PersistencyAndSerialization;
using UnityEngine;

namespace SaveSystem.Front
{
    public class SavesInitializer : MonoBehaviour
    {
        [SerializeField] private Serializers _serializer;
        [SerializeField] private PersistencyProvider _persistencyProvider;
        [SerializeField] private InitializationType _initializationType;
        private bool _startedInitializing;
        
        public async void Awake()
        {
            if (_initializationType != InitializationType.Automatic || AlreadyInitialized()) Destroy(this);

            await Initialize();
        }

        public Task<Saves> InitializeOnDemand()
        {
            if (_initializationType != InitializationType.OnDemand)
            {
                throw new InvalidOperationException(
                    $"This {nameof(SavesInitializer)}'s initialization type is not set to {nameof(InitializationType.OnDemand)}");
            }

            return Initialize();
        }
        
        private async Task<Saves> Initialize()
        {
            Saves saves = InitializeSavesObject();
            DontDestroyOnLoad(saves);

            bool initialized = false;
            saves.ExecuteWhenReady(() =>
            {
                initialized = true;
            });
            while (!initialized) await Task.Yield();
            return saves;
        }
        
        private IPersistencyProvider<ISaveDataSerializer> BuildPersistencyProvider(
            PersistencyProvider persistencyProvider,
            ISaveDataSerializer serializer)
        {
            switch (persistencyProvider)
            {
                case PersistencyProvider.LocalFile:
                    return new LocalFilePersistency<ISaveDataSerializer>(serializer);
                    break;
                case PersistencyProvider.Google:
                    throw new NotImplementedException();
                    break;
                default:
                    return new LocalFilePersistency<ISaveDataSerializer>(serializer);
            }
        }

        private ISaveDataSerializer BuildSerializer(Serializers serializer)
        {
            switch (serializer)
            {
                case Serializers.Binary:
                    return new BinarySaveDataSerializer();
                    break;
                default:
                    return new BinarySaveDataSerializer();
            }
        }

        private Saves InitializeSavesObject()
        {
            ISaveDataSerializer serializer = BuildSerializer(_serializer);
            IPersistencyProvider<ISaveDataSerializer> persistencyProvider = BuildPersistencyProvider(_persistencyProvider, serializer);
            
            GameObject gameObject = new GameObject { name = "Saves" };
            Saves saves = gameObject.AddComponent<Saves>();
            saves.Initialize(persistencyProvider);
            return saves;
        }
        
        private bool AlreadyInitialized()
        {
            return FindObjectOfType<Saves>() is not null;
        }

        private enum InitializationType
        {
            Automatic,
            OnDemand
        }
        
        private enum Serializers
        {
            Binary
        }

        private enum PersistencyProvider
        {
            LocalFile,
            Google
        }
    }
}