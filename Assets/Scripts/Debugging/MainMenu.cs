using IGUIDResources;
using LevelLoading;
using UnityEngine;

namespace Debugging
{
    public class MainMenu : MonoBehaviour
    {
        public void StartLevel(int number)
        {
            LevelLoader levelLoader = new LevelLoader();
            levelLoader.LoadLevelWithBikeSelection(GUIDResourceLocator.Initialize().Career.Chapters[0].Levels[number].GetGUID());
        }
    }
}
