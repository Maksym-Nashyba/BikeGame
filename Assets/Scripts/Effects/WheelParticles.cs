using System;
using Misc;
using TMPro;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class WheelParticles : MonoBehaviour
    {
        [SerializeField] private GameObject _bicycleGameObject;
        [SerializeField] private float _speedEmissionMultiplier = 75f;
        [SerializeField] private float _accelerationEmissionMultiplier = 100f;
        [SerializeField] private float _torqueEmissionMultiplier = 10f;
        private ParticleSystem _particleSystem;
        private Renderer _particleRenderer;
        private Transform _currentLandscape;
        private MeshRenderer _currentLandscapeRenderer;
        private Transform _transform;
        private IBicycle _bicycle;
        private RaycastHit[] _cachedHits;

        private const float RaycastLength = 0.3f;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _particleSystem = GetComponent<ParticleSystem>();
            _bicycle = _bicycleGameObject.GetComponent<IBicycle>();
            _cachedHits = new RaycastHit[1];
            _particleRenderer = GetComponent<Renderer>();
        }

        private void FixedUpdate()
        {
            if (!TouchesGround(out RaycastHit hit))
            {
                SetParticlesEmissionRate(0f);
                return;
            }

            if (_currentLandscape != hit.transform)
            {
                _currentLandscape = hit.transform;
                _currentLandscapeRenderer = _currentLandscape.GetComponent<MeshRenderer>();
            }

            Color landscapeColor = GetLandscapeColor(hit);
            SetParticlesColor(landscapeColor);
            SetParticlesEmissionRate(CalculateEmissionRate());
        }

        private float CalculateEmissionRate()
        {
            float speedPercent = _bicycle.GetCurrentSpeed() / 14f;
            if (speedPercent < 0.3f) return 0;

            float fromSpeed = _speedEmissionMultiplier * speedPercent * speedPercent;
            float fromAcceleration = _accelerationEmissionMultiplier * Mathf.Abs(_bicycle.GetAcceleration());
            float fromTorque = _torqueEmissionMultiplier * Mathf.Abs(_bicycle.GetTorqueY());

            return fromSpeed + fromAcceleration + fromTorque;
        }

        private bool TouchesGround(out RaycastHit hit)
        {
            Ray ray = new Ray(_transform.position + Vector3.up * 0.06f, Vector3.down * RaycastLength);
            int hits = Physics.RaycastNonAlloc(ray, _cachedHits, RaycastLength, LayerMask.GetMask("Landscape"));
            hit = _cachedHits[0];
            return hits > 0;
        }

        private Color GetLandscapeColor(RaycastHit hit)
        {
            Texture2D texture = _currentLandscapeRenderer.material.mainTexture as Texture2D;
            Vector2 pixelOnTexture = hit.textureCoord;
            pixelOnTexture.x *= texture.width;
            pixelOnTexture.y *= texture.height;

            return texture.GetPixel((int)pixelOnTexture.x, (int)pixelOnTexture.y);
        }

        private void SetParticlesEmissionRate(float rate)
        {
            ParticleSystem.EmissionModule particleSystemEmission = _particleSystem.emission;
            particleSystemEmission.rateOverTime = rate;
        }

        private void SetParticlesColor(Color color)
        {
            color /= 1.2f;
            _particleRenderer.material.color = color;
        }
    }
}