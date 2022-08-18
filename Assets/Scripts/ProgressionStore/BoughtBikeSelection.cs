using System;
using IGUIDResources;
using UnityEngine;

namespace ProgressionStore
{
    public class BoughtBikeSelection : MonoBehaviour
    {
        private GUIDResourceLocator _resourceLocator;
        private Garage _garage;
        int _currentBikeIndex;

        private void Awake()
        {
            _garage = FindObjectOfType<Garage>();
            _resourceLocator = GUIDResourceLocator.Initialize();
        }

        public void SwitchToNextBike(bool rightDirection)
        {
            if(rightDirection) _currentBikeIndex += 1;
            else _currentBikeIndex -= 1;

            _garage.SelectBike(_resourceLocator.Bikes.Models[_currentBikeIndex]);
        }
    }
}