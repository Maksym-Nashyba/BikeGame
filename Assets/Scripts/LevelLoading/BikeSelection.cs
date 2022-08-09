using System;
using System.Threading.Tasks;
using IGUIDResources;
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
        
        private GUIDResourceLocator _resourceLocator;
        private BikeModel[] _bikes;
        private BikeModel _currentBike;
        private GameObject _spawnedBike;
        private int _currentIndex;
        
        private void Awake()
        {
            _resourceLocator = GUIDResourceLocator.Initialize();
            _bikes = _resourceLocator.Bikes.Models;
        }
        
        private void Start()
        {
            _currentIndex = 0;
            DisplayBike(_bikes[_currentIndex]);
        }
        
        public void ShowNextBike()
        {
            if (_currentIndex >= _bikes.Length - 1) return;
            
            _currentIndex++;
            DisplayBike(_bikes[_currentIndex]);
        }
        
        public void ShowPreviousBike()
        {
            if (_currentIndex <= 0) return;

            _currentIndex--;
            DisplayBike(_bikes[_currentIndex]);
        }
        
        private void DisplayBike(BikeModel bike)
        {
            if (_spawnedBike is not null)
            {
                Destroy(_spawnedBike);
            }
            
            _currentBike = bike;
            _spawnedBike = Instantiate(_currentBike.Prefab);
            BikeChanged?.Invoke(_currentBike);
        }
        
        private void Update()
        {
            if (_currentIndex == 0)
            {
                _previousButton.interactable = false;
            }
            else
            {
                _previousButton.interactable = true;
            }

            if (_currentIndex >= _bikes.Length - 1)
            {
                _nextButton.interactable = false;
            }
            else
            {
                _nextButton.interactable = true;
            }
        }

        public void SelectBike()
        {
            RetrieveSelectedBikeModel();
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
        
        public Task<BikeModel> RetrieveSelectedBikeModel()
        {
            return Task.FromResult(_currentBike);
        }
    }
}