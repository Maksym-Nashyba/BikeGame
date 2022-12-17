using SaveSystem.Front;
using UnityEngine;

namespace Menu
{
    public class DonationPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        private const string _playerPrefsKey = "DonationPanelShown";
        private Saves _saves;

        private void Awake()
        {
            _saves = FindObjectOfType<Saves>();
        }

        private void Start()
        {
            if (GameComplete() && !AlreadyShown())
            {
                Show();
            }
        }

        public void Hide()
        {
            _panel.SetActive(false);
        }

        public void OnDonateButton()
        {
            Hide();
        }
        
        private void Show()
        {
            PlayerPrefs.SetInt(_playerPrefsKey, 1);
            PlayerPrefs.Save();
            _panel.SetActive(true);            
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