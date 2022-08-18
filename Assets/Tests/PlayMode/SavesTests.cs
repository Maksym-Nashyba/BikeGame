using System.Collections;
using NUnit.Framework;
using SaveSystem.Front;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class SaveSystemTests
    {
        [UnityTest]
        public IEnumerator Initialize()
        {
            InstantiateSavesInitializer();
            Saves saves = Object.FindObjectOfType<Saves>();
            while (!saves.IsValid) yield return null;
            Assert.Pass();
        }

        [UnityTest]
        public IEnumerator SaveOneLoadOne()
        {
            yield return null;
            GameObject initializerObject = InstantiateSavesInitializer().gameObject;
            Saves writingSaves = Object.FindObjectOfType<Saves>();
            while (!writingSaves.IsValid) yield return null;
            writingSaves.ClearSaves();
            writingSaves.Currencies.AddDollans(47);
            while (writingSaves.IsSaving) yield return null;
            Object.Destroy(writingSaves.gameObject);
            Object.Destroy(initializerObject);
            
            initializerObject = InstantiateSavesInitializer().gameObject;
            Saves readingSaves = Object.FindObjectOfType<Saves>();
            while (!readingSaves.IsValid) yield return null;
            Assert.IsTrue(readingSaves.Currencies.GetDollans() == 47, $"actual: {readingSaves.Currencies.GetDollans()}");
            Object.Destroy(initializerObject);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator SaveMultipleLoadLast()
        {
            yield return null;
            GameObject initializerObject = InstantiateSavesInitializer().gameObject;
            Saves writingSaves = Object.FindObjectOfType<Saves>();
            while (!writingSaves.IsValid) yield return null;
            writingSaves.ClearSaves();
            writingSaves.Currencies.AddDollans(550);
            writingSaves.Currencies.SubtractDollans(550);
            writingSaves.Currencies.AddDollans(50);
            writingSaves.Currencies.AddDollans(50);
            while (writingSaves.IsSaving) yield return null;
            Object.Destroy(writingSaves.gameObject);
            Object.Destroy(initializerObject);
            
            initializerObject = InstantiateSavesInitializer().gameObject;
            Saves readingSaves = Object.FindObjectOfType<Saves>();
            while (!readingSaves.IsValid) yield return null;
            Assert.IsTrue(readingSaves.Currencies.GetDollans() == 100, $"actual: {readingSaves.Currencies.GetDollans()}");
            Object.Destroy(initializerObject);
            yield return null;
        }
        
        private SavesInitializer InstantiateSavesInitializer()
        {
            return new GameObject().AddComponent<SavesInitializer>();
        }
    }
}
