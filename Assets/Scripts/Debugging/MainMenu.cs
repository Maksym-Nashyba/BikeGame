using IGUIDResources;
using SaveSystem.Front;
using UnityEngine;

namespace Debugging
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            GUIDResourceLocator resourceLocator = GUIDResourceLocator.Initialize();
            Saves saves = FindObjectOfType<Saves>();
        }
    }
}
