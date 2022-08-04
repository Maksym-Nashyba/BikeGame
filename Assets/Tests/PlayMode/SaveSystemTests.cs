using System.Collections;
using SaveSystem.Front;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class SaveSystemTests
    {
        [UnityTest]
        public IEnumerator Initialize()
        {
            Saves saves = Saves.Initialize();
            yield return null;
        }
    }
}
