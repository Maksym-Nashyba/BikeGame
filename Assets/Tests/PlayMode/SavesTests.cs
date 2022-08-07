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
            InstantiateInitializer();
            Saves saves = Object.FindObjectOfType<Saves>();
            while (!saves.IsValid) yield return null;
            Assert.Pass();
        }

        private SavesInitializer InstantiateInitializer()
        {
            return new GameObject().AddComponent<SavesInitializer>();
        }
    }
}
