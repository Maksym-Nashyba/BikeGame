using UnityEngine;

namespace Gameplay.GameCamera
{
    [RequireComponent(typeof(MeshRenderer))]
    public class CameraObstruction : MonoBehaviour
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _transparentMaterial;
        private static readonly int Oppacity = Shader.PropertyToID("_Oppacity");
        private MeshRenderer _renderer;
        private float _timeTillSwitchToDefault;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void MakeTransparentForSomeTime()
        {
            SwitchToMaterial(_transparentMaterial);
            _timeTillSwitchToDefault = 0.1f;
            float nextOppacity = _transparentMaterial.GetFloat(Oppacity);
            nextOppacity = Mathf.Clamp(nextOppacity - Time.deltaTime, 0.3f, 1f);
            _transparentMaterial.SetFloat(Oppacity, nextOppacity);
        }

        private void Update()
        {
            if (_timeTillSwitchToDefault < 0) return;

            _timeTillSwitchToDefault -= Time.deltaTime;
            
            if (_timeTillSwitchToDefault < 0)
            {
                SwitchToMaterial(_defaultMaterial);
                _timeTillSwitchToDefault = 0;
                _transparentMaterial.SetFloat(Oppacity, 0.8f);
            }
        }

        private void SwitchToMaterial(Material material)
        {
            _renderer.material = material;
        }
    }
}