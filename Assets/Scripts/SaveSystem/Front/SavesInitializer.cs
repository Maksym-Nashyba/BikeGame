using System;
using SaveSystem.PersistencyAndSerialization;
using UnityEngine;

namespace SaveSystem.Front
{
    public class SavesInitializer : MonoBehaviour
    {
        [SerializeField] private Serializers _serializer;
        [SerializeField] private PersistencyProvider _persistencyProvider;

        private void Awake()
        {
            if (AlreadyInitialized()) return;
            
            ISaveDataSerializer serializer = BuildSerializer(_serializer);
            IPersistencyProvider<ISaveDataSerializer> persistencyProvider = BuildPersistencyProvider(_persistencyProvider, serializer);
            Persistency persistency = new Persistency(persistencyProvider);
            Saves saves = InstantiateObjectWithSavesComponent();
            
        }

        private IPersistencyProvider<ISaveDataSerializer> BuildPersistencyProvider(PersistencyProvider persistencyProvider, ISaveDataSerializer serializer)
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

        private Saves InstantiateObjectWithSavesComponent()
        {
            GameObject gameObject = new GameObject();
            return gameObject.AddComponent<Saves>();
        }
        
        private bool AlreadyInitialized()
        {
            return FindObjectOfType<Saves>() is not null;
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