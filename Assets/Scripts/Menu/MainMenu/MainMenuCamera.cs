using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Misc;
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
        private bool _isMoving;
        private AsyncExecutor _asyncExecutor;

        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
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
            _asyncExecutor.Dispose();
        }

        public async Task RotateToTarget(Targets target, float duration = -1f)
        {
            if(_isMoving) return;
            _isMoving = true;
            
            Quaternion startRotation = _cameraTransform.rotation;
            Quaternion targetRotation = _targetRotations[target];
            duration = duration < 0 ? _transitionDuration : duration;

            await _asyncExecutor.EachFrame(duration, t =>
            {
                _cameraTransform.rotation = Quaternion.LerpUnclamped(startRotation, targetRotation, t);
            }, EaseFunctions.InOutBack);

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