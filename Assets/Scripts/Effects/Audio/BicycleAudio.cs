using System;
using Gameplay;
using Misc;
using Pausing;
using UnityEngine;

namespace Effects.Audio
{
    public class BicycleAudio : MonoBehaviour, IPausable
    {
        [SerializeField] private AudioSource _tyresAudioSource;
        [SerializeField] private GameObject _bicycleGameobject;
        private IBicycle _bicycle;

        private void Awake()
        {
            _bicycle = _bicycleGameobject.GetComponent<IBicycle>();
            ServiceLocator.Player.Died += OnPlayerDied;
        }

        private void Start()
        {
            _tyresAudioSource.volume = 0f;
        }

        private void FixedUpdate()
        {
            if (_bicycle.IsAirborne())
            {
                _tyresAudioSource.volume = 0f;
                return;
            }
            
            _tyresAudioSource.volume = EaseFunctions.EaseInCirc(_bicycle.GetCurrentSpeed() / 20f);
        }

        private void OnDestroy()
        {
            ServiceLocator.Player.Died -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            Destroy(gameObject);
        }
        
        public void Pause()
        {
            _tyresAudioSource.Pause();
        }

        public void Continue()
        {
            _tyresAudioSource.Play();
        }
    }
}