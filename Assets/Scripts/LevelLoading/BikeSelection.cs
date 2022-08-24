using System;
using System.Threading.Tasks;
using IGUIDResources;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelLoading
{
    public class BikeSelection : MonoBehaviour
    {
        public event Action<BikeModel> BikeChanged;
        
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;

        private Saves _saves;
        private GUIDResourceLocator _resourceLocator;
        private PersistentBike[] _persistentBikes;
        private BikeModels _bikeModels;
        private BikeModel _currentBike;
        private GameObject _spawnedBike;
        private int _currentIndex = 0;
        private TaskCompletionSource<BikeModel> _taskCompletionSource;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _persistentBikes = _saves.Bikes.GetAllUnlockedBikes();
            _resourceLocator = GUIDResourceLocator.Initialize();
            _bikeModels = _resourceLocator.Bikes;
        }

        private void OnEnable()
        {
            DisplayBike(_persistentBikes[_currentIndex]);
        }

        public void ShowNextBike()
        {
            if (_currentIndex >= _persistentBikes.Length - 1) return;
            
            _currentIndex++;
            DisplayBike(_persistentBikes[_currentIndex]);
        }
        
        public void ShowPreviousBike()
        {
            if (_currentIndex <= 0) return;

            _currentIndex--;
            DisplayBike(_persistentBikes[_currentIndex]);
        }
        
        private void DisplayBike(PersistentBike bike)
        {
            if (_spawnedBike is not null)
            {
                Destroy(_spawnedBike);
            }
            
            _currentBike = _bikeModels.Get(bike.GUID);
            _spawnedBike = Instantiate(_currentBike.Prefab);
            BikeChanged?.Invoke(_currentBike);
        }
        
        private void Update()
        {
            UpdateButtonsStatus(_currentIndex, _persistentBikes.Length);
        }
        
        private void UpdateButtonsStatus(int currentIndex, int levelsCount)
        {
            _nextButton.interactable = currentIndex < levelsCount - 1;
            _previousButton.interactable = currentIndex > 0;
        }

        public void SelectBike()
        {
            _taskCompletionSource.SetResult(_currentBike);
        }
        
        public static async Task<BikeSelection> DisplayBikeSelection()
        {
            AsyncOperation loadingProcess = SceneManager.LoadSceneAsync("BikeSelection", LoadSceneMode.Additive);
            while (!loadingProcess.isDone)
            {
                await Task.Yield();
            }
            return FindObjectOfType<BikeSelection>();
        }
        
        public void RegisterTaskCompletionSource(TaskCompletionSource<BikeModel> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }
    }
}