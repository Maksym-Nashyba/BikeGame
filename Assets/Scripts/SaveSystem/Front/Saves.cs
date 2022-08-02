using IGUIDResources;
using SaveSystem.PersistencyAndSerialization;

namespace SaveSystem.Front
{
    public class Saves
    {
        public SavedBikes Bikes { get; private set; }
        public SavedCurrencies Currencies { get; private set; }
        public SavedCareer Career { get; private set; }
        private Persistency _persistency;

        private Saves(Persistency persistency, GUIDResourceLocator resources)
        {
            _persistency = persistency;
            Bikes = new SavedBikes(persistency, resources);
            Career = new SavedCareer(persistency, resources);
            Currencies = new SavedCurrencies(persistency);
        }

        public static Saves Initialize()
        {
            GUIDResourceLocator resources = GUIDResourceLocator.Initialize();
            IPersistencyProvider<ISaveDataSerializer> persistencyProvider = BuildPersistencyProvider();
            Persistency persistency = new Persistency(persistencyProvider);
            Saves result = new Saves(persistency, resources);
            persistency.Pull();
            return result;
        }

        private static IPersistencyProvider<ISaveDataSerializer> BuildPersistencyProvider()
        {
            return new DebugPersistencyProvider();
        }
    }
}