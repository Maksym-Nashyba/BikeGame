using Gameplay;
using LevelObjectives.LevelObjects;
using Misc;
using UnityEngine;

namespace LevelSpecific.IcePeak
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class WeatherChanger : PlayerTrigger
    {
        [SerializeField] private int _maxEmissionRate = 175;
        [SerializeField] private ParticleSystem _snowParticles;
        [SerializeField] private ParticleSystem _rainParticles;
        [SerializeField] private Transform _coldSide;
        [SerializeField] private Transform _warmSide;
        [SerializeField] private AudioSource _coldAudio;
        [SerializeField] private AudioSource _warmAudio;
        private float _coldAudioMaxVolume;
        private float _warmAudioMaxVolume;
        private Vector3 _coldToWarm;
        private float _coldToWarmDistance;

        private void Start()
        {
            _coldToWarm = _warmSide.position - _coldSide.position;
            _coldToWarmDistance = _coldToWarm.magnitude;
            _coldAudioMaxVolume = _coldAudio.volume;
            _warmAudioMaxVolume = _warmAudio.volume;
            _warmAudio.volume = 0f;
            
            ServiceLocator.Player.Respawned += OnPlayerRespawned;
        }

        private void OnTriggerStay(Collider other)
        {
            float snowDistance01 = (_coldSide.transform.position - other.transform.position).sqrMagnitude /
                                   (_coldToWarmDistance * _coldToWarmDistance);
            float warmDistance01 = (_warmSide.transform.position - other.transform.position).sqrMagnitude /
                                   (_coldToWarmDistance * _coldToWarmDistance);
            SetParticlesRate(_snowParticles, 1f-snowDistance01);
            SetParticlesRate(_rainParticles, 1f-warmDistance01);
            _coldAudio.volume = (1f-snowDistance01).Remap(0f,1f,0f,_coldAudioMaxVolume);
            _warmAudio.volume = (1f-warmDistance01).Remap(0f,1f,0f,_warmAudioMaxVolume);
        }
        
        private void SetParticlesRate(ParticleSystem particleSystem, float portion)
        {
            ParticleSystem.EmissionModule particleSystemEmission = particleSystem.emission;
            particleSystemEmission.rateOverTime = Mathf.Clamp(_maxEmissionRate*portion -5f, 0, float.MaxValue);
        }

        private void OnPlayerRespawned()
        {
            if (IsPlayerInColdZone())
            {
                _coldAudio.volume = _coldAudioMaxVolume;
                _warmAudio.volume = 0f;
                SetParticlesRate(_snowParticles, 1f);
                SetParticlesRate(_rainParticles, 0f);
            }
            else
            {
                _coldAudio.volume = 0f;
                _warmAudio.volume = _warmAudioMaxVolume;
                SetParticlesRate(_snowParticles, 0f);
                SetParticlesRate(_rainParticles, 1f);
            }
        }

        private bool IsPlayerInColdZone()
        {
            Transform playerTransform = ServiceLocator.Player.ActivePlayerClone.transform;
            return Vector3.Distance(playerTransform.position, _coldSide.position) <
                   Vector3.Distance(playerTransform.position, _warmSide.position);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.Player.Respawned -= OnPlayerRespawned;
        }
    }
}