using UnityEngine;

namespace Effects
{
    public class WheelParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private IBicycle _bicycle;
        private int _fixUpdateNumber;
        private float _currentSpeed;

        private void Awake()
        {
            _bicycle = GetComponent<IBicycle>();
        }

        private void FixedUpdate()
        {
            _currentSpeed = _bicycle.GetCurrentSpeed();
            
            
        }
    }
}
