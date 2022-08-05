using IGUIDResources;
using SaveSystem.PersistencyAndSerialization;
using UnityEngine;

namespace SaveSystem.Front
{
    public class Saves : MonoBehaviour
    {
        public SavedBikes Bikes { get; private set; }
        public SavedCurrencies Currencies { get; private set; }
        public SavedCareer Career { get; private set; }
        private Persistency _persistency;
        private bool _isValid = false;
        
        public void Initialize(Persistency persistency)
        {
            GUIDResourceLocator resourceLocator = GUIDResourceLocator.Initialize();
            _persistency = persistency;
            Bikes = new SavedBikes(persistency, resourceLocator);
            Career = new SavedCareer(persistency, resourceLocator);
            Currencies = new SavedCurrencies(persistency);
            _isValid = true;
        }
    }
}