using System;
using System.Threading.Tasks;
using SaveSystem.Front;
using UnityEngine;

namespace Menu.Garage.Computer.Browser
{
    public class BikeHub : MonoBehaviour
    {
        [SerializeField] private int _rewardDollans;
        private Computer _computer;

        private void Awake()
        {
            _computer = FindObjectOfType<Computer>();
        }

        public async void OnPlayButton()
        {
            switch (await ShowAdd())
            {
                case AddShowResult.Shown:
                    _computer.ShowDialog($"THANKS, IT HELPS A LOT\nREWARD_{_rewardDollans}");
                    FindObjectOfType<Saves>().Currencies.AddDollans(_rewardDollans);
                    break;
                case AddShowResult.Failed:
                    _computer.ShowDialog("FAILED\nPLEASE_TRY_AGAIN_LATER");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<AddShowResult> ShowAdd()
        {
            await Task.Delay(1000);
            return AddShowResult.Shown;
        }

        private enum AddShowResult
        {
            Shown,
            Failed
        }
    }
}
