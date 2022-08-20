using IGUIDResources;
using LevelLoading;
using SaveSystem.Front;
using UnityEngine;
using UnityEngine.UI;

namespace Debugging
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;

        private void Start()
        {
            GUIDResourceLocator resourceLocator = GUIDResourceLocator.Initialize();
            Saves saves = FindObjectOfType<Saves>();
            for (int i = 1; i < _buttons.Length; i++)
            {
                string guid = resourceLocator.Career.Chapters[0][i-1].GetGUID();
                _buttons[i].interactable = saves.Career.IsCompleted(guid);
            }

            _buttons[0].interactable = true;
        }

        public async void StartLevel(int number)
        {
            LevelLoader levelLoader = new LevelLoader();
            string levelGUID = GUIDResourceLocator.Initialize().Career.Chapters[0].Levels[number].GetGUID();
            await levelLoader.LoadLevelWithBikeSelection(levelGUID);
        }
    }
}
