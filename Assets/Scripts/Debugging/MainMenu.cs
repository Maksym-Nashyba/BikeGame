using IGUIDResources;
using LevelLoading;
using UnityEngine;

namespace Debugging
{
    public class MainMenu : MonoBehaviour
    {
        public async void StartLevel(int number)
        {
            LevelLoader levelLoader = new LevelLoader();
            string levelGUID = GUIDResourceLocator.Initialize().Career.Chapters[0].Levels[number].GetGUID();
            await levelLoader.LoadLevelWithBikeSelection(levelGUID);
        }
    }
}
