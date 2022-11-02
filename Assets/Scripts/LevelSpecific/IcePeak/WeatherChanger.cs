using LevelObjectives.LevelObjects;
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
        private Vector3 _coldToWarm;
        private float _coldToWarmDistance;

        private void Start()
        {
            _coldToWarm = _warmSide.position - _coldSide.position;
            _coldToWarmDistance = _coldToWarm.magnitude;
        }

        private void SetParticlesRate(ParticleSystem particleSystem, float portion)
        {
            ParticleSystem.EmissionModule particleSystemEmission = particleSystem.emission;
            particleSystemEmission.rateOverTime = Mathf.Clamp(_maxEmissionRate*portion -5f, 0, float.MaxValue);
        }

        private void OnTriggerStay(Collider other)
        {
            SetParticlesRate(_snowParticles, 1f-(_coldSide.transform.position - other.transform.position).sqrMagnitude/(_coldToWarmDistance * _coldToWarmDistance));
            SetParticlesRate(_rainParticles, 1f-(_warmSide.transform.position - other.transform.position).sqrMagnitude/(_coldToWarmDistance * _coldToWarmDistance));
        }
    }
}