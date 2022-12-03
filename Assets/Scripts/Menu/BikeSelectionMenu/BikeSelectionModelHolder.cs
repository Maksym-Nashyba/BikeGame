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
            await MoveToFront();
            Arrived?.Invoke();
        }

        public Task MoveToFront()
        {
            return MoveToPosition(_frontPosition.position, _duration);
        }

        public Task MoveToBack()
        {
            return MoveToPosition(_backPosition.position, _duration);
        }

        public async Task Rotate(Direction1D direction, Action midRotationCallback)
        {
            Quaternion startRotation = HolderTransform.localRotation;
            int directionMultiplier = direction == Direction1D.Right ? 1 : -1;
            
            Task rotation = _asyncExecutor.EachFrame(0.6f, t =>
            {
                HolderTransform.localRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y + 360f * t * directionMultiplier, startRotation.eulerAngles.z);
            }, EaseFunctions.InOutBack);
            await Task.Delay(300);
            midRotationCallback.Invoke();
            await rotation;
        }
    }
}