using System;
using IGUIDResources;
using Misc;
using SaveSystem.Front;
using UnityEngine;

namespace Garage
{
    public class BikePreview : MonoBehaviour
    {
        [SerializeField] private CameraCheckpoint _checkpoint;
        [SerializeField] private GarageUI _garageUI;
        [SerializeField] private BikeModelDisplay _display;
        private Saves _saves;
        private BikeModel _currentModel;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
            _checkpoint.CameraArrived += OnCameraArrived;
            _checkpoint.CameraDeparted += OnCameraDeparted;
        }

        private void OnDestroy()
        {
            _checkpoint.CameraArrived -= OnCameraArrived;
            _checkpoint.CameraDeparted -= OnCameraDeparted;
        }

        private void OnCameraArrived()
        {
            _garageUI.RightLeftButtonClicked += OnNavigationButton;
        }

        private void OnCameraDeparted()
        {
            _garageUI.RightLeftButtonClicked -= OnNavigationButton;
        }
        
        private void OnNavigationButton(Direction1D direction)
        {
            _currentModel = PickNextModel(direction);
            _display.Display(_currentModel, _saves.Bikes.GetSelectedSkinFor(_currentModel));
        }

        private BikeModel PickNextModel(Direction1D direction)
        {
            throw new NotImplementedException();
        }
    }
}