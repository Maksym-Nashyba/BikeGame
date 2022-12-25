using Gameplay;
using Misc;
using UnityEngine;

namespace Effects.Audio
{
    public class BicycleAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _tyresAudioSource;
        [SerializeField] private GameObject _bicycleGameobject;
        private IBicycle _bicycle;

        private void Awake()
        {
            _bicycle = _bicycleGameobject.GetComponent<IBicycle>();
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
    }
}