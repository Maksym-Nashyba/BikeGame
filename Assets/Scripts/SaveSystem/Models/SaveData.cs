using System;
using IGUIDResources;

namespace SaveSystem.Models
{
    [Serializable]
    public class SaveData
    {
        public byte Version;
        public PersistentLevel[] CareerLevels;
        public PersistentBike[] Bikes;
        public PersistentCurrencies Currencies;
        private const byte _currentVerion = 1;
        
        public SaveData(PersistentLevel[] careerLevels, PersistentBike[] bikes, PersistentCurrencies currencies)
        {
            CareerLevels = careerLevels;
            Bikes = bikes;
            Currencies = currencies;
            Version = _currentVerion;
        }

        public static SaveData GetDefault()
        {
            GUIDResourceLocator resources = GUIDResourceLocator.Initialize();
            return new SaveData(new []{PersistentLevel.GetNewLevelWithGUID(resources.Career.GetFirstLevel().GetGUID())},
                new []{resources.Bikes.GetDefault().MakeCleanSaveObject()},
                new PersistentCurrencies(0, 0));
        }

        public override bool Equals(object obj)
        {
            if (obj is not SaveData other) return false;
            if (Version != other.Version || !Equals(Currencies, other.Currencies)) return false;

            if (CareerLevels.Length != other.CareerLevels.Length) return false;
            if (Bikes.Length != other.Bikes.Length) return false;
            for (int i = 0; i < CareerLevels.Length; i++)
            {
                if (!Equals(CareerLevels[i], other.CareerLevels[i])) return false;
            }
            for (int i = 0; i < Bikes.Length; i++)
            {
                if (!Equals(Bikes[i], other.Bikes[i])) return false;
            }
            return true;
        }
    }
}