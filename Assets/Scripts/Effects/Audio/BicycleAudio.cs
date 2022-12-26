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
        [SerializeField] private GameObject _bicycleGameobject;
        private AudioSource[] _audioSources;
        private IBicycle _bicycle;

        private void Awake()
        {
            _audioSources = transform.GetComponentsInChildren<AudioSource>();
            _bicycle = _bicycleGameobject.GetComponent<IBicycle>();
            ServiceLocator.Player.Died += OnPlayerDied;
        }

        private void Start()
        {
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
        }

        private void OnPlayerDied()
        {
            Destroy(gameObject);
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