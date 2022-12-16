using System;
using SaveSystem.Front;
using UnityEngine;

namespace Menu
{
    public class DonationPanel : MonoBehaviour
    {
        private const string _playerPrefsKey = "DonationPanelShown";
        private Saves _saves;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
        }

        private void Start()
        {
            if (GameComplete() && AlreadyShown())
            {
                Show();
            }
        }

        private void Show()
        {
            throw new NotImplementedException();
        }

        private bool AlreadyShown()
        {
            return PlayerPrefs.HasKey(_playerPrefsKey);
        }

        private bool GameComplete()
        {
            return _saves.Career.GetAllCompletedLevels().Length >= 4;
        }
    }
}