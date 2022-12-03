using System;
using System.Threading.Tasks;
using IGUIDResources;
using Misc;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.BikeSelectionMenu
{
    public class BikeSelection : MonoBehaviour
    {
        public event Action<BikeModel> BikeChanged;
        
        [SerializeField] private BikeSelectionModelHolder _bikeHolder;
        [SerializeField] private BikeSelectionUI _bikeSelectionUI;
        
        private Saves _saves;
        private GUIDResourceLocator _resourceLocator;
        private PersistentBike[] _persistentBikes;
        private BikeModels _bikeModels;
        private BikeModel _currentBike;
        private GameObject _spawnedBike;
        private int _currentIndex;
        private TaskCompletionSource<BikeModel> _taskCompletionSource;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _persistentBikes = _saves.Bikes.GetAllUnlockedBikes();
            _resourceLocator = GUIDResourceLocator.Initialize();
            _bikeModels = _resourceLocator.Bikes;
        }

        private async void Start()
        {
            _bikeSelectionUI.SetUIState(false);
            BikeChanged?.Invoke(_currentBike);
            await _bikeSelectionUI.ShowUI();
            _bikeSelectionUI.SetUIState(true);
        }

        private void OnEnable()
        {
            DisplayBike(_persistentBikes[_currentIndex]);
        }

        private void Update()
        {
            UpdateButtonsStatus(_currentIndex, _persistentBikes.Length);
        }
        
        public async void ShowNextBike()
        {
            if (_currentIndex >= _persistentBikes.Length - 1) return;
            
            _currentIndex++;
            _bikeSelectionUI.SetUIState(false);
            await _bikeHolder.Rotate(Direction1D.Right, () =>
            {
                DisplayBike(_persistentBikes[_currentIndex]);
            });
            _bikeSelectionUI.SetUIState(true);
        }
        
        public async void ShowPreviousBike()
        {
            if (_currentIndex <= 0) return;

            _currentIndex--;
            _bikeSelectionUI.SetUIState(false);
            await _bikeHolder.Rotate(Direction1D.Left, () =>
            {
                DisplayBike(_persistentBikes[_currentIndex]);
            });
            _bikeSelectionUI.SetUIState(true);
        }

        public async void SelectBike()
        {
            _bikeSelectionUI.SetUIState(false);
            await Task.WhenAll(
                _bikeHolder.MoveToBack(),
                _bikeSelectionUI.HideUI());
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
        
        private void DisplayBike(PersistentBike bike)
        {
            if (_spawnedBike is not null) Destroy(_spawnedBike);
            _currentBike = _bikeModels.Get(bike.GUID);
            _spawnedBike = Instantiate(_currentBike.EmptyPrefab, _bikeHolder.HolderTransform);
            BikeChanged?.Invoke(_currentBike);
        }

        private void UpdateButtonsStatus(int currentIndex, int levelsCount)
        {
            _bikeSelectionUI.SetButtonState(Direction1D.Right, currentIndex < levelsCount - 1);
            _bikeSelectionUI.SetButtonState(Direction1D.Left, currentIndex > 0);
        }
    }
}