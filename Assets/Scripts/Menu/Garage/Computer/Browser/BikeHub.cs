using System;
using System.Threading;
using System.Threading.Tasks;
using SaveSystem.Front;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Menu.Garage.Computer.Browser
{
    public class BikeHub : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private int _rewardDollans;
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _loadCircle;
        private Computer _computer;
        private string _rewardVideoID;
        private CancellationTokenSource _cancellationTokenSource;
        private TaskCompletionSource<Result> _loadCompletionSource;
        private TaskCompletionSource<Result> _showCompletionSource;

        private void Awake()
        {
            _rewardVideoID = Application.platform == RuntimePlatform.IPhonePlayer
                ? "Rewarded_iOS"
                : "Rewarded_Android";
            
            _cancellationTokenSource = new CancellationTokenSource();
            _computer = FindObjectOfType<Computer>();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async void OnPlayButton()
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            ShowLoadingCircle();
            if (await LoadAd() == Result.Failed)
            {
                ResetPlayer();
                ShowErrorMessage();
                return;
            }
            
            if(cancellationToken.IsCancellationRequested)return;
            
            switch (await ShowAd())
            {
                case Result.Success:
                    ShowRewardMessage(_rewardDollans);        
                    FindObjectOfType<Saves>().Currencies.AddDollans(_rewardDollans);
                    break;
                case Result.Failed:
                    ShowErrorMessage();
                    break;
            }
            ResetPlayer();
        }

        private Task<Result> ShowAd()
        {
            _showCompletionSource = new TaskCompletionSource<Result>();
            Advertisement.Show(_rewardVideoID, this);
            return _showCompletionSource.Task;
        }

        private Task<Result> LoadAd()
        {
            _loadCompletionSource = new TaskCompletionSource<Result>();
            Advertisement.Load(_rewardVideoID, this);
            return _loadCompletionSource.Task;
        }

        private void ShowRewardMessage(int rewardDollans)
        {
            _computer.ShowDialog($"THANKS, IT HELPS A LOT\nREWARD_{rewardDollans}");
        }
        
        private void ShowErrorMessage()
        {
            _computer.ShowDialog("FAILED\nPLEASE_TRY_AGAIN_LATER");
        }

        private void ResetPlayer()
        {
            _playButton.SetActive(true);
            _loadCircle.SetActive(false);
        }

        private void ShowLoadingCircle()
        {
            _playButton.SetActive(false);
            _loadCircle.SetActive(true);
        }

        private enum Result
        {
            Success,
            Failed
        }

        #region AdsCallbacks
        public void OnUnityAdsAdLoaded(string placementId)
        {
            _loadCompletionSource.SetResult(Result.Success);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            _loadCompletionSource.SetResult(Result.Failed);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            _showCompletionSource.SetResult(Result.Failed);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Result result = showCompletionState == UnityAdsShowCompletionState.COMPLETED
                ? Result.Success
                : Result.Failed;
            _showCompletionSource.SetResult(result);
        }
        #endregion
    }
}
