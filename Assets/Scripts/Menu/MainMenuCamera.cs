using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenuCamera : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        [Space]
        [SerializeField] private Quaternion _mainRotation;
        [SerializeField] private Quaternion _settingsRotation;
        [SerializeField] private Quaternion _topRotation;
        [SerializeField] private float _transitionDuration;
        private Dictionary<Targets, Quaternion> _targetRotations;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isMoving;

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _targetRotations = new Dictionary<Targets, Quaternion>(3)
            {
                { Targets.Main, _mainRotation },
                { Targets.Settings, _settingsRotation },
                { Targets.Top, _topRotation }
            };
            _mainMenu.SettingsButtonPressed += async () => await RotateToTarget(Targets.Settings);
            _settingsMenu.BackButtonClicked += async () => await RotateToTarget(Targets.Main);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async Task RotateToTarget(Targets target, float duration = -1f)
        {
            if(_isMoving) return;
            _isMoving = true;
            CancellationToken token = _cancellationTokenSource.Token;
            Quaternion startRotation = _cameraTransform.rotation;
            Quaternion targetRotation = _targetRotations[target];
            duration = duration < 0 ? _transitionDuration : duration;
            
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                _cameraTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime/duration);
                await Task.Yield();
                if(token.IsCancellationRequested) break;
                elapsedTime += Time.deltaTime;
            }

            _isMoving = false;
        }

        public enum Targets
        {
            Main,
            Settings,
            Top
        }
    }
}