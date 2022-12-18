using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace SetUp
{
    public class AdsSetUp : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string _androidGameId;
        [SerializeField] string _iOSGameId;
        [SerializeField] bool _testMode = true;
        private string _gameId;
        private TaskCompletionSource<bool> _completionSource;

        private void Awake()
        {
            _completionSource = new TaskCompletionSource<bool>();
            SetUpOperation setUpOperation = new SetUpOperation(InitializeSaves, "Ads Loaded", false);
            FindObjectOfType<GameSetUp>().RegisterSetUpTask(setUpOperation);
        }

        public void OnInitializationComplete()
        {
            _completionSource.SetResult(true);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            _completionSource.SetException(new Exception($"Failed to innit saves: {error}, {message}"));
        }

        private Task InitializeSaves()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, this);
            Task timeOutDelay = Task.Delay(10000);
            return Task.WhenAny(_completionSource.Task, timeOutDelay);
        }
    }
}