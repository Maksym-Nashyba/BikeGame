using System;
using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Menu.BikeSelectionMenu
{
    public class BikeSelectionModelHolder : BikeModelHolder
    {
        public event Action Arrived;
        [SerializeField] private Transform _backPosition;
        [SerializeField] private Transform _frontPosition;
        [SerializeField] private float _duration;

        private async void Start()
        {
            HolderTransform.position = _backPosition.position;
            await MoveToFront(_duration);
            Arrived?.Invoke();
        }

        private Task MoveToFront(float duration)
        {
            return MoveToPosition(_frontPosition.position, duration);
        }
    }
}