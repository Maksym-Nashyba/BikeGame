using System;
using Garage;
using IGUIDResources;
using Misc;
using SaveSystem.Front;
using SaveSystem.Models;
using UnityEngine;

namespace Menu.Garage
{
    public class BikePreview : MonoBehaviour
    {
        [SerializeField] private CameraCheckpoint _checkpoint;
        [SerializeField] private GarageUI _garageUI;
        [SerializeField] private BikeModelDisplay _display;
        private Saves _saves;
        private BikeModel _currentModel;
        private GUIDResourceLocator _resourceLocator;

        private void Awake()
        {
            _resourceLocator = GUIDResourceLocator.InitializeBikes();
            _saves = FindObjectOfType<Saves>();
            _checkpoint.CameraArrived += OnCameraArrived;
            _checkpoint.CameraDeparted += OnCameraDeparted;
        }

        private void Start()
        {
            _currentModel = PickFirstModel();
            _display.Display(_currentModel, _saves.Bikes.GetSelectedSkinFor(_currentModel));
        }

        private void OnDestroy()
        {
            _checkpoint.CameraArrived -= OnCameraArrived;
            _checkpoint.CameraDeparted -= OnCameraDeparted;
        }

        private void OnCameraArrived()
        {
            _garageUI.RightLeftButtonClicked += OnNavigationButton;
            _garageUI.SetRightLeftButtonsActive(true);
        }

        private void OnCameraDeparted()
        {
            _garageUI.RightLeftButtonClicked -= OnNavigationButton;
            _garageUI.SetRightLeftButtonsActive(false);
        }

        private void OnNavigationButton(Direction1D direction)
        {
            _currentModel = PickNextModel(direction);
            _display.Display(_currentModel, _saves.Bikes.GetSelectedSkinFor(_currentModel));
        }

        private BikeModel PickNextModel(Direction1D direction)
        {
            PersistentBike[] allUnlockedBiked = _saves.Bikes.GetAllUnlockedBikes();
            int currentIndex = Array.FindIndex(allUnlockedBiked, bike => bike.GUID == _currentModel.GetGUID());
            int nextIndex = GetNextIndexWithWraparound(allUnlockedBiked.Length, currentIndex, direction);
            return _resourceLocator.Bikes.Get(allUnlockedBiked[nextIndex].GUID);
        }

        private int GetNextIndexWithWraparound(int length, int current, Direction1D direction)
        {
            if (length == 1) return current;
            if (direction == Direction1D.Left)
            {
                if (current == 0) return --length;
                return --current;
            }

            if (current == length - 1) return 0;
            return ++current;
        }

        private BikeModel PickFirstModel()
        {
            return _resourceLocator.Bikes.Get(_saves.Bikes.GetAllUnlockedBikes()[0].GUID);
        }
    }
}