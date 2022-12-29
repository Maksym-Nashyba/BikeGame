using Gameplay;
using Misc;
using Pausing;
using UnityEngine;

namespace Effects.Audio
{
    public class BicycleAudio : MonoBehaviour, IPausable
    {
        [SerializeField] private AudioSource _tyresAudioSource;
        [SerializeField] private AudioSource _windAudioSource;
        [SerializeField] private AudioSource _brakesAudioSource;
        [SerializeField] private AudioSource _fallAudioSource;
        [SerializeField] private GameObject _bicycleGameobject;
        private AudioSource[] _audioSources;
        private IBicycle _bicycle;

        private void Awake()
        {
            _audioSources = transform.GetComponentsInChildren<AudioSource>();
            _bicycle = _bicycleGameobject.GetComponent<IBicycle>();
        }

        private void Start()
        {
            ServiceLocator.Player.Died += OnPlayerDied;
            _bicycle.Landed += OnPlayerLanded;
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.volume = 0;
            }
        }

        private void Update()
        {
            if (_bicycle.IsAirborne())
            {
                _tyresAudioSource.volume = 0f;
            }
            else
            {
                _tyresAudioSource.volume = EaseFunctions.EaseInCirc(_bicycle.GetCurrentSpeed() / 20f);
            }
            _windAudioSource.volume = EaseFunctions.InOutQuad((_bicycle.GetAirtimeSeconds() / 2f).Remap(0,1,0,0.5f));
        }

        private void OnDestroy()
        {
            ServiceLocator.Player.Died -= OnPlayerDied;
            _bicycle.Landed -= OnPlayerLanded;
        }

        private void OnPlayerDied()
        {
            Destroy(gameObject);
        }
        
        private void OnPlayerLanded()
        {
            if(_bicycle.IsAirborne() 
               || _fallAudioSource.isPlaying
               || Mathf.Abs(_bicycle.GetCurrentVelocity().y) > 0.5f) return;
            _fallAudioSource.volume = 0.15f;
            _fallAudioSource.Play();
        }

        public void Pause()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.Pause();
            }
            _tyresAudioSource.Pause();
        }

        public void Continue()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.Play();
            }
        }
    }
}