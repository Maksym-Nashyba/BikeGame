using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Menu.Garage
{
    public class ClickHint : MonoBehaviour
    {
        [SerializeField] private string _prefsKey;
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;
        private Transform _transform;
        
        private void Awake()
        {
            _cameraCheckpoint.CameraApproaching += OnCameraApproaching;
            _transform = GetComponent<Transform>();
        }

        private void OnDestroy()
        {
            _cameraCheckpoint.CameraApproaching -= OnCameraApproaching;
        }

        private void Start()
        {
            if (AlreadyClicked())
            {
                Destroy(gameObject);
            }
        }

        private async void OnCameraApproaching()
        {
            await PlayShrinkingAnimation();
            PlayerPrefs.SetInt(_prefsKey, 0);
            PlayerPrefs.Save();
            Destroy(gameObject);
        }

        private async Task PlayShrinkingAnimation()
        {
            using AsyncExecutor asyncExecutor = new AsyncExecutor();
            Vector3 startScale = _transform.localScale;
            await asyncExecutor.EachFrame(0.5f, t =>
            {
                _transform.localScale = startScale * (1f - t);
            },EaseFunctions.InOutQuad);
        }

        private bool AlreadyClicked()
        {
            return PlayerPrefs.HasKey(_prefsKey);
        }
    }
}