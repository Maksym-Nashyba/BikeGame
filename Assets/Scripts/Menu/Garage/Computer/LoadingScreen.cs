using System.Threading.Tasks;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Menu.Garage.Computer
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private float _duration;
        
        [SerializeField] private CanvasGroup _screen;
        [SerializeField] private CanvasGroup _background;
        [SerializeField] private CanvasGroup _elements;
        [SerializeField] private CameraCheckpoint _cameraCheckpoint;
        [SerializeField] private Slider _loadingBarSlider;
        [SerializeField] private UnityEvent StartedLoading;
        
        private void Awake()
        {
            _cameraCheckpoint.CameraArrived += OnCameraArrived;
        }

        private void Start()
        {
            _elements.alpha = 0f;
        }

        private void OnDestroy()
        {
            _cameraCheckpoint.CameraArrived -= OnCameraArrived;
        }

        private async void OnCameraArrived()
        {
            await Play(_duration);
            Destroy(gameObject);
        }
        
        private async Task Play(float durationSeconds)
        {
            StartedLoading.Invoke();
            _elements.alpha = 1f;
            float loadingBarDuration = durationSeconds * 0.8f;
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay((int)(1000 * (loadingBarDuration/10f)));
                _loadingBarSlider.value = (i + 1f) / 10f;
            }
            using AsyncExecutor asyncExecutor = new AsyncExecutor();
            asyncExecutor.EachFrame(durationSeconds * 0.1f, t =>
            {
                _elements.alpha = 1f - t;
            }, EaseFunctions.InOutQuad);
            await asyncExecutor.EachFrame(durationSeconds * 0.2f, t =>
            {
                _screen.alpha = 1f - t;
            }, EaseFunctions.InOutQuad);
        }
    }
}