using System;
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

        private Saves(Persistency persistency)
        {
            _persistency = persistency;
        }

        public static Saves Initialize()
        {

            throw new NotImplementedException();
            GUIDResourceLocator resources = GUIDResourceLocator.Initialize();
            IPersistencyProvider<BinarySaveDataSerializer> persistencyProvider = null;
            Persistency persistency = new Persistency(persistencyProvider);
            Saves result = new Saves(persistency);
            persistency.Pull();
            result.Bikes = new SavedBikes(persistency, resources);
            result.Career = new SavedCareer(persistency, resources);
            result.Currencies = new SavedCurrencies(persistency);
            return result;
        }
    }
}