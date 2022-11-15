using System;
using UnityEngine;

namespace ProgressionStore
{
    public class GarageUI : MonoBehaviour
    {
        public event Action BackButtonClicked;
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