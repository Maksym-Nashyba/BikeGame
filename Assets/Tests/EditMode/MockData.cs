using System;
using System.Linq;
using IGUIDResources;
using SaveSystem.Models;
using SaveSystem.PersistencyAndSerialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests.EditMode
{
    public static class MockData
    {
        public static SaveData GenerateMockSaveData()
        {
            PersistentLevel[] levels = {GenerateRandomPersistentLevel(), GenerateRandomPersistentLevel()};
            PersistentBike[] bikes = {GenerateRandomPersistentBike()};
            PersistentCurrencies currencies = new PersistentCurrencies(4, 5);
            return new SaveData(levels, bikes, currencies);
        }

        public static byte[] GenerateMockSerializedData()
        {
            BinarySaveDataSerializer serializer = new BinarySaveDataSerializer();
            if (!serializer.TrySerialize(GenerateMockSaveData(), out byte[] result).Result)
                throw new Exception("Failed to serialize mock data");
            return result;
        }

        public static PersistentLevel GenerateRandomPersistentLevel()
        {
            float bestTime = Random.Range(3f, 6f);
            bool pedalCollected = Random.Range(0, 1) > 0.5f;
            string guid = GUIDResourceLocator.Initialize().Career.Chapters[0][Random.Range(1, 3)].GetGUID();
            PersistentLevel level = PersistentLevel.GetNewLevelWithGUID(guid);
            level.BestTime = bestTime;
            level.PedalCollected = pedalCollected;
            return level;
        }

        public static PersistentBike GenerateRandomPersistentBike()
        {
            GUIDResourceLocator resources = GUIDResourceLocator.Initialize();
            BikeModel bikeModel = resources.Bikes.Models[0];
            string[] unlockedSkins = new string[Mathf.Clamp(bikeModel.AllSkins.Length - Random.Range(0,2), 1, Int32.MaxValue)];
            unlockedSkins[0] = bikeModel.AllSkins[0].GetGUID();
            for (int i = 1; i < unlockedSkins.Length; i++)
            {
                int guess = Random.Range(1, bikeModel.AllSkins.Length);
                int direction = 1;
                while (unlockedSkins.Contains(bikeModel.AllSkins[guess].GetGUID()))
                {
                    if (guess == bikeModel.AllSkins.Length - 1) direction = -1;
                    guess += direction;
                }
                unlockedSkins[i] = bikeModel.AllSkins[guess].GetGUID();
            }
            return new PersistentBike(unlockedSkins[0],unlockedSkins,bikeModel.GetGUID());
        }
    }
}