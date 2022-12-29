using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Effects.Audio
{
    public class AmbientAudio : MonoBehaviour
    {
        public AmbientType Type => _ambientType;
        [SerializeField] private AmbientType _ambientType;
        [SerializeField] private AudioSource _audioSource;
        private AsyncExecutor _asyncExecutor;

        private void Awake()
        {
            _asyncExecutor = new AsyncExecutor();
        }

        private async void Start()
        {
            AmbientAudio[] allAudios = FindObjectsOfType<AmbientAudio>();
            foreach (AmbientAudio ambientAudio in allAudios)
            {
                if(ambientAudio == this) continue;
                if (ambientAudio.Type == Type)
                {
                    Destroy(gameObject);
                }
                else
                {
                    await ambientAudio.MuteOverTime(1f);
                    Destroy(ambientAudio.gameObject);
                }
            }

            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            _audioSource.Play();
        }

        private void OnDestroy()
        {
            _asyncExecutor.Dispose();
        }

        public Task MuteOverTime(float durationSeconds)
        {
            float startVolume = _audioSource.volume;
            return _asyncExecutor.EachFrame(durationSeconds, t =>
            {
                _audioSource.volume = (1f - t).Remap(0f, 1f, 0f, startVolume);
            }, EaseFunctions.InOutQuad);
        }

        public enum AmbientType
        {
            Menu,
            Level
        }
    }
}