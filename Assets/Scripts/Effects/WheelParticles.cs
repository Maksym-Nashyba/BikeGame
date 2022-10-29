using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class WheelParticles : MonoBehaviour
    {
        [SerializeField] private GameObject _bicycleGameObject;
        private ParticleSystem _particleSystem;
        private Transform _transform;
        private IBicycle _bicycle;
        private RaycastHit[] _cachedHits;
        
        private const float RaycastLength = 0.2f;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _particleSystem = GetComponent<ParticleSystem>();
            _bicycle = _bicycleGameObject.GetComponent<IBicycle>();
            _cachedHits = new RaycastHit[1];
        }

        private void FixedUpdate()
        {
            if (!TouchesGround(out RaycastHit hit))
            {
                SetParticlesEmissionRate(0f);    
                return;
            }

            Debug.DrawLine(_transform.position, hit.point, Color.green);
            Color landscapeColor = GetLandscapeColor(hit);
            SetParticlesColor(landscapeColor);
            SetParticlesEmissionRate(30f * _bicycle.GetCurrentSpeed()/20f + Mathf.Clamp(_bicycle.GetAcceleration(), 0f, float.MaxValue) * 3f);
        }

        private bool TouchesGround(out RaycastHit hit)
        {
            Ray ray = new Ray(_transform.position, Vector3.down * RaycastLength);
            int hits = Physics.RaycastNonAlloc(ray, _cachedHits, RaycastLength, LayerMask.GetMask("Landscape"));
            hit = _cachedHits[0];
            return hits > 0;
        }

        private Color GetLandscapeColor(RaycastHit hit)
        {
            return Color.magenta;
        }

        private void SetParticlesEmissionRate(float rate)
        {
            ParticleSystem.EmissionModule particleSystemEmission = _particleSystem.emission;
            particleSystemEmission.rateOverTime = rate;
        }
        
        private void SetParticlesColor(Color color)
        {
            ParticleSystem.MainModule particleSystemMain = _particleSystem.main;
            particleSystemMain.startColor = color;
        }
    }
}
