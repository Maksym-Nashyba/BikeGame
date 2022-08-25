using System;
using IGUIDResources;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace ProgressionStore
{
    public class BoughtBikeSelection : MonoBehaviour
    {
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        private GUIDResourceLocator _resourceLocator;
        private Garage _garage;
        int _currentBikeIndex;

        private void Awake()
        {
            _garage = FindObjectOfType<Garage>();
            _resourceLocator = GUIDResourceLocator.Initialize();
            UpdateButtonInteractability();
        }

        public void SwitchToNextBike(bool rightDirection)
        {
            if(rightDirection) _currentBikeIndex += 1;
            else _currentBikeIndex -= 1;
            UpdateButtonInteractability();

            _garage.SelectBike(_resourceLocator.Bikes.Models[_currentBikeIndex]);
        }

        private void UpdateButtonInteractability()
        {
            _rightButton.interactable = _currentBikeIndex != _resourceLocator.Bikes.Models.Length - 1;
            _leftButton.interactable = _currentBikeIndex != 0;
        }
    }
}