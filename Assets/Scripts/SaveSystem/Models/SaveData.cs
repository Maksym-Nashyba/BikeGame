using System;
using IGUIDResources;

namespace SaveSystem.Models
{
    [Serializable]
    public class SaveData
    {
        public PersistentLevel[] CareerLevels;
        public PersistentBike[] Bikes;
        public PersistentCurrencies Currencies;

        public SaveData(PersistentLevel[] careerLevels, PersistentBike[] bikes, PersistentCurrencies currencies)
        {
            CareerLevels = careerLevels;
            Bikes = bikes;
            Currencies = currencies;
        }

        public static SaveData GetDefault()
        {
            GUIDResourceLocator resources = GUIDResourceLocator.Initialize();
            return new SaveData(new []{PersistentLevel.GetNewLevelWithGUID("3UR5PKXKM")},
                new []{resources.Bikes.GetDefault().MakeCleanSaveObject()},
                new PersistentCurrencies(5000, 5000));
        }
    }
}