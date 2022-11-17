using System;
using Misc;
using ProgressionStore;
using UnityEngine;

namespace Garage
{
    public class GarageUI : MonoBehaviour
    {
        public event Action BackButtonClicked;
        public event Action<Direction1D> RightLeftButtonClicked; 
        [SerializeField] private GameObject _backButton;
        [SerializeField] private GarageCamera _garageCamera;

        private void Awake()
        {
            _backButton.SetActive(false);
            _garageCamera.ArrivedAtCheckpoint += OnCameraArrivedAtCheckpoint;
            _garageCamera.DepartedFromCheckpoint += OnCameraDepartedFromCheckpoint;
        }

        private void OnDestroy()
        {
            _garageCamera.ArrivedAtCheckpoint -= OnCameraArrivedAtCheckpoint;
            _garageCamera.DepartedFromCheckpoint -= OnCameraDepartedFromCheckpoint;
        }

        public void OnBackButton()
        {
            BackButtonClicked?.Invoke();
        }

        public void OnRightLeftButtons(Direction1D direction)
        {
            RightLeftButtonClicked?.Invoke(direction);
        }
        
        private void OnCameraArrivedAtCheckpoint()
        {
            _backButton.SetActive(!_garageCamera.IsAtRestPoint);
        }
        
        private void OnCameraDepartedFromCheckpoint()
        {
            _backButton.SetActive(false);
        }
    }
}